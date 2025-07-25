using NotifyHub.Abstractions.Enums;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using NotifyHub.TelegramBot.Domain.DTOs;
using NotifyHub.TelegramBot.Infrastructure.Managers;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class CreateNotificationMessageHandler(
    NotificationCreationSessionManager sessionManager,
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
        sessionManager.HasDraft(msg.From!.Id);

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var userId = msg.From!.Id;
        var draft = sessionManager.GetOrCreateDraft(userId);
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
            sessionManager.RemoveDraft(userId);
            userStateService.SetState(userId, UserState.Default);
            await bot.SendMessage(userId, "Создание уведомления отменено.", replyMarkup: keyboard,  cancellationToken: token);
            return;
        }
        
        var cancelMarkup = new ReplyKeyboardMarkup([
            [new KeyboardButton("Отмена")]
        ])
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

        switch (draft.Step)
        {
            case NotificationCreationStep.Title:
                draft.Title = text;
                draft.Step = NotificationCreationStep.Description;
                await bot.SendMessage(userId, "Введите описание уведомления:", replyMarkup: cancelMarkup, cancellationToken: token);
                break;

            case NotificationCreationStep.Description:
                draft.Description = text;
                draft.Step = NotificationCreationStep.Type;
                await bot.SendMessage(userId, "Введите тип уведомления: ONE_TIME или RECURRING", replyMarkup: typeKeyboard, cancellationToken: token);
                break;

            case NotificationCreationStep.Type:
            {
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
                draft.Step = NotificationCreationStep.ScheduledAt;

                await bot.SendMessage(
                    chatId: userId,
                    text: "Введите дату и время (в формате: dd.MM.yyyy HH:mm)",
                    replyMarkup: cancelMarkup,
                    cancellationToken: token
                );
                break;
            }

            case NotificationCreationStep.ScheduledAt:
                if (!DateTime.TryParseExact(text, "dd.MM.yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out var dt))
                {
                    await bot.SendMessage(userId, "Неверный формат. Попробуйте снова: dd.MM.yyyy HH:mm", replyMarkup: cancelMarkup, cancellationToken: token);
                    return;
                }

                draft.ScheduledAt = dt;

                if (draft.Type == "RECURRING")
                {
                    draft.Step = NotificationCreationStep.Frequency;

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
                    draft.Step = NotificationCreationStep.Confirm;
                    await ConfirmAndCreate(userId, draft, bot, token);
                }

                break;

            case NotificationCreationStep.Frequency:
                if (!FrequencyMap.TryGetValue(text!, out var freq))
                {
                    await bot.SendMessage(userId, "Пожалуйста, выберите одну из предложенных периодичностей.", cancellationToken: token);
                    return;
                }

                draft.Frequency = (int)freq;
                draft.Step = NotificationCreationStep.Confirm;
                await ConfirmAndCreate(userId, draft, bot, token);
                break;

            case NotificationCreationStep.Confirm:
                await bot.SendMessage(userId, "Уведомление уже создаётся. Подождите...", cancellationToken: token);
                break;
        }
    }

    private async Task ConfirmAndCreate(long userId, CreateNotificationDraft draft, ITelegramBotClient bot, CancellationToken token)
    {
        var dto = new CreateNotificationDto
        {
            Title = draft.Title!,
            Description = draft.Description!,
            Type = NotificationTypeMap[draft.Type!],
            ScheduledAt = draft.ScheduledAt!.Value,
            Frequency = draft.Type == "RECURRING"
                ? (NotificationFrequency?)draft.Frequency!.Value
                : null
        };

        await notificationService.CreateNotificationAsync(userId, dto, token);
        sessionManager.RemoveDraft(userId);
        userStateService.SetState(userId, UserState.Default);
        
        var keyboard = new ReplyKeyboardMarkup([
            ["Получить уведомления", "Помощь"]
        ])
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        };

        await bot.SendMessage(userId, "Уведомление успешно создано!", replyMarkup: keyboard, cancellationToken: token);
    }
}