using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class HelpMessageHandler : IMessageHandler
{
    public bool CanHandle(Message msg, UserState state) =>
        msg.Text == "Помощь" || msg.Text?.Trim().ToLower() == "/help";

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken token)
    {
        var helpText = """
           <b>Помощь</b>

           Этот бот помогает создавать и управлять уведомлениями.

           <b>Что умеет бот:</b>
           • Создание уведомлений с датой и описанием
           • Редактирование и удаление уведомлений
           • Просмотр истории отправок

           <b>Доступные команды:</b>
           /start — регистрация и запуск
           /help — показать это сообщение

           <b>Кнопки на клавиатуре:</b>
           <b>Создать</b> — начать создание нового уведомления
           <b>Изменить</b> — выбрать уведомление для редактирования
           <b>Удалить</b> — выбрать уведомление для удаления
           <b>История</b> — посмотреть историю отправленных уведомлений
           <b>Уведомления</b> — показать список всех твоих уведомлений

           Если что-то не работает — просто напиши “/start” чтобы сбросить состояние.
        """;

        await bot.SendMessage(
            msg.Chat,
            helpText,
            parseMode: Telegram.Bot.Types.Enums.ParseMode.Html,
            cancellationToken: token);
    }
}