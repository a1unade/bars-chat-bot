using MediatR;
using NotifyHub.TelegramBot.Application.Features.Queries.GetNotificationHistory;
using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class HistoryMessageHandler(IMediator mediator) : IMessageHandler
{
    public bool CanHandle(Message msg, UserState state) =>
        msg.Text?.Trim().ToLower() == "история";

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var userId = msg.From!.Id;

        var logs = await mediator.Send(new GetNotificationHistoryQuery(userId), token);

        if (logs.Count == 0)
        {
            await bot.SendMessage(msg.Chat.Id, "История пуста.", cancellationToken: token);
            return;
        }

        var text = string.Join("\n\n", logs.Select(log => 
            $@"<b>{log.Title ?? "Без названия"}</b>
        {log.Description ?? "Без описания"}

        <i>{log.SentAt:dd.MM.yyyy HH:mm}</i>
        Ошибка: {(string.IsNullOrWhiteSpace(log.ErrorMessage) ? "Нет" : log.ErrorMessage)}"));

        await bot.SendMessage(
            msg.Chat.Id,
            $"<b>История уведомлений:</b>\n\n{text}",
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
            cancellationToken: token);
    }
}
