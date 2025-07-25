using NotifyHub.Abstractions.Enums;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using NotifyHub.TelegramBot.Domain.DTOs;
using NotifyHub.TelegramBot.Infrastructure.Managers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class UpdateNotificationMessageHandler(
    NotificationUpdateSessionManager sessionManager,
    INotificationService notificationService,
    IUserStateService userStateService)
    : IMessageHandler
{
    private static readonly Dictionary<string, NotificationFrequency> FrequencyMap = new(StringComparer.OrdinalIgnoreCase)
    {
        ["Каждый час"] = NotificationFrequency.Hourly,
        ["Каждый день"] = NotificationFrequency.Daily,
        ["Каждую неделю"] = NotificationFrequency.Weekly,
        ["Каждый месяц"] = NotificationFrequency.Monthly,
        ["Каждый год"] = NotificationFrequency.Yearly
    };

    private static readonly Dictionary<string, NotificationType> NotificationTypeMap = new()
    {
        { "ONE_TIME", NotificationType.OneTime },
        { "RECURRING", NotificationType.Recurring }
    };

    public bool CanHandle(Message msg, UserState state) =>
        state == UserState.UpdatingNotification && sessionManager.HasSession(msg.From!.Id);

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var userId = msg.From!.Id;
        var draft = sessionManager.GetDraft(userId);
        var text = msg.Text?.Trim();
        
        var keyboard = new ReplyKeyboardMarkup([
            ["Получить уведомления", "Помощь"]
        ])
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        };

        if (text?.Equals("Отмена", StringComparison.OrdinalIgnoreCase) == true)
        {
            sessionManager.RemoveSession(userId);
            userStateService.SetState(userId, UserState.Default);
            await bot.SendMessage(userId, "Редактирование уведомления отменено.", replyMarkup: keyboard, cancellationToken: token);
            return;
        }

        var cancelMarkup = new ReplyKeyboardMarkup([ [new KeyboardButton("Отмена")] ])
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        };
        
        var typeKeyboard = new ReplyKeyboardMarkup([
            [new KeyboardButton("ONE_TIME"), new KeyboardButton("RECURRING")],
            [new KeyboardButton("Отмена")]
        ])
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = true
        };

        if (draft.Step == NotificationUpdateStep.AskTitle && !sessionManager.HasSelectedId(userId))
        {
            if (!int.TryParse(text, out var number))
            {
                await bot.SendMessage(userId, "Неверный номер. Попробуй снова.", replyMarkup: cancelMarkup, cancellationToken: token);
                return;
            }

            sessionManager.SetSelected(userId, number - 1);
            await bot.SendMessage(userId, "Введите новое название уведомления:", replyMarkup: cancelMarkup, cancellationToken: token);
            return;
        }

        switch (draft.Step)
        {
            case NotificationUpdateStep.AskTitle:
                draft.Title = text;
                draft.Step = NotificationUpdateStep.AskDescription;
                await bot.SendMessage(userId, "Введите новое описание:", replyMarkup: cancelMarkup, cancellationToken: token);
                break;

            case NotificationUpdateStep.AskDescription:
                draft.Description = text;
                draft.Step = NotificationUpdateStep.AskType;
                await bot.SendMessage(userId, "Введите тип: ONE_TIME или RECURRING", replyMarkup: typeKeyboard, cancellationToken: token);
                break;

            case NotificationUpdateStep.AskType:
                if (text is not ("ONE_TIME" or "RECURRING"))
                {

                    await bot.SendMessage(
                        chatId: userId,
                        text: "Неверный тип. Выберите ONE_TIME или RECURRING:",
                        replyMarkup: typeKeyboard,
                        cancellationToken: token
                    );
                    return;
                }
                
                draft.Type = text;
                draft.Step = NotificationUpdateStep.AskScheduledAt;

                await bot.SendMessage(
                    chatId: userId,
                    text: "Введите дату и время (в формате: dd.MM.yyyy HH:mm)",
                    replyMarkup: cancelMarkup,
                    cancellationToken: token
                );
                break;

            case NotificationUpdateStep.AskScheduledAt:
                if (!DateTime.TryParseExact(text, "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out var dt))
                {
                    await bot.SendMessage(userId, "Неверный формат. Попробуйте снова: dd.MM.yyyy HH:mm", replyMarkup: cancelMarkup, cancellationToken: token);
                    return;
                }

                draft.ScheduledAt = dt;

                if (draft.Type == "RECURRING")
                {
                    draft.Step = NotificationUpdateStep.AskFrequency;

                    var freqKeyboard = new ReplyKeyboardMarkup([
                        [new KeyboardButton("Каждый час"), new KeyboardButton("Каждый день")],
                        [new KeyboardButton("Каждую неделю"), new KeyboardButton("Каждый месяц")],
                        [new KeyboardButton("Каждый год"), new KeyboardButton("Отмена")]
                    ])
                    {
                        ResizeKeyboard = true,
                        OneTimeKeyboard = true
                    };

                    await bot.SendMessage(userId, "Выберите периодичность уведомления:", replyMarkup: freqKeyboard, cancellationToken: token);
                }
                else
                {
                    draft.Step = NotificationUpdateStep.Confirm;
                    await ConfirmAndUpdate(userId, draft, bot, token);
                }

                break;

            case NotificationUpdateStep.AskFrequency:
                if (!FrequencyMap.TryGetValue(text!, out var freq))
                {
                    await bot.SendMessage(userId, "Пожалуйста, выберите одну из предложенных периодичностей.", cancellationToken: token);
                    return;
                }

                draft.Frequency = (int)freq;
                draft.Step = NotificationUpdateStep.Confirm;
                await ConfirmAndUpdate(userId, draft, bot, token);
                break;

            case NotificationUpdateStep.Confirm:
                await bot.SendMessage(userId, "Уведомление уже обновляется. Подождите...", cancellationToken: token);
                break;
        }
    }

    private async Task ConfirmAndUpdate(long userId, UpdateNotificationDraft draft, ITelegramBotClient bot, CancellationToken token)
    {
        var dto = new UpdateNotificationDto
        {
            Id = sessionManager.GetSelectedId(userId),
            Title = draft.Title,
            Description = draft.Description,
            Type = NotificationTypeMap[draft.Type!],
            ScheduledAt = draft.ScheduledAt,
            Frequency = draft.Type == "RECURRING"
                ? (NotificationFrequency?)draft.Frequency!.Value
                : null
        };

        await notificationService.UpdateNotificationAsync(dto, token);

        sessionManager.RemoveSession(userId);
        userStateService.SetState(userId, UserState.Default);
        
        var keyboard = new ReplyKeyboardMarkup([
            ["Получить уведомления", "Помощь"]
        ])
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        };

        await bot.SendMessage(userId, "Уведомление успешно обновлено!", replyMarkup: keyboard, cancellationToken: token);
    }
}
