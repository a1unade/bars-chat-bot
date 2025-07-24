using MediatR;
using Microsoft.Extensions.Logging;
using NotifyHub.TelegramBot.Application.Features.Commands.DeleteUserNotification;
using NotifyHub.TelegramBot.Application.Features.Queries.GetUserNotifications;
using NotifyHub.TelegramBot.Domain.DTOs;
using NotifyHub.TelegramBot.Domain.Events;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class UpdateHandler(
    ITelegramBotClient bot, 
    ILogger<UpdateHandler> logger,
    IMediator mediator) : IUpdateHandler
{
    private static readonly InputPollOption[] PollOptions = ["Hello", "World!"];
    private readonly Dictionary<long, List<Guid>> _pendingNotificationDeletions = new();

    public async Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken)
    {
        logger.LogInformation("HandleError: {Exception}", exception);
        // Cooldown in case of network connection error
        if (exception is RequestException)
            await Task.Delay(TimeSpan.FromSeconds(2), cancellationToken);
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        await (update switch
        {
            { Message: { } message }                        => OnMessage(message, cancellationToken),
            { EditedMessage: { } message }                  => OnMessage(message, cancellationToken),
            { CallbackQuery: { } callbackQuery }            => OnCallbackQuery(callbackQuery, cancellationToken),
            { InlineQuery: { } inlineQuery }                => OnInlineQuery(inlineQuery),
            { ChosenInlineResult: { } chosenInlineResult }  => OnChosenInlineResult(chosenInlineResult),
            { Poll: { } poll }                              => OnPoll(poll),
            { PollAnswer: { } pollAnswer }                  => OnPollAnswer(pollAnswer),
            // UpdateType.ChannelPost:
            // UpdateType.EditedChannelPost:
            // UpdateType.ShippingQuery:
            // UpdateType.PreCheckoutQuery:
            _                                               => UnknownUpdateHandlerAsync(update)
        });
    }

    private async Task OnMessage(Message msg, CancellationToken cancellationToken)
    {
        logger.LogInformation("Receive message type: {MessageType}", msg.Type);
        if (msg.Text is not { } messageText)
            return;
        
        messageText = messageText switch
        {
            "📨 Получить уведомления" => "/notifications",
            "ℹ️ Помощь" => "/help",
            _ => messageText
        };
        
        if (_pendingNotificationDeletions.TryGetValue(msg.From!.Id, out var notificationIds))
        {
            if (int.TryParse(msg.Text, out int index) && index >= 1 && index <= notificationIds.Count)
            {
                var notificationIdToDelete = notificationIds[index - 1];

                await mediator.Send(new DeleteUserNotificationCommand
                {
                    NotificationId = notificationIdToDelete
                }, cancellationToken);

                _pendingNotificationDeletions.Remove(msg.From.Id);

                await bot.SendMessage(msg.Chat, "✅ Уведомление удалено.", cancellationToken: cancellationToken);
            }
            else
            {
                await bot.SendMessage(msg.Chat, "❌ Введён некорректный номер. Попробуйте снова.", cancellationToken: cancellationToken);
            }

            return;
        }
        
        var command = messageText.Split(' ')[0];

        Message sentMessage = await (command switch
        {
            "/start" => HandleGreetings(msg, cancellationToken),
            "/help" => SendHelp(msg),
            "/notifications" => SendNotifications(msg),
            "/photo" => SendPhoto(msg),
            "/inline_buttons" => SendInlineKeyboard(msg),
            "/keyboard" => SendReplyKeyboard(msg),
            "/remove" => RemoveKeyboard(msg),
            "/request" => RequestContactAndLocation(msg),
            "/inline_mode" => StartInlineQuery(msg),
            "/poll" => SendPoll(msg),
            "/poll_anonymous" => SendAnonymousPoll(msg),
            "/throw" => FailingHandler(msg),
            _ => Usage(msg)
        });

        logger.LogInformation("The message was sent with id: {SentMessageId}", sentMessage.Id);
    }

    async Task<Message> Usage(Message msg)
    {
        const string usage = """
                <b><u>Bot menu</u></b>:
                /photo          - send a photo
                /inline_buttons - send inline buttons
                /keyboard       - send keyboard buttons
                /remove         - remove keyboard buttons
                /request        - request location or contact
                /inline_mode    - send inline-mode results list
                /poll           - send a poll
                /poll_anonymous - send an anonymous poll
                /throw          - what happens if handler fails
            """;
        return await bot.SendMessage(msg.Chat, usage, parseMode: ParseMode.Html, replyMarkup: new ReplyKeyboardRemove());
    }

    async Task<Message> SendPhoto(Message msg)
    {
        await bot.SendChatAction(msg.Chat, ChatAction.UploadPhoto);
        await Task.Delay(2000); // simulate a long task
        await using var fileStream = new FileStream("Files/bot.gif", FileMode.Open, FileAccess.Read);
        return await bot.SendPhoto(msg.Chat, fileStream, caption: "Read https://telegrambots.github.io/book/");
    }

    // Send inline keyboard. You can process responses in OnCallbackQuery handler
    async Task<Message> SendInlineKeyboard(Message msg)
    {
        var inlineMarkup = new InlineKeyboardMarkup()
            .AddNewRow("1.1", "1.2", "1.3")
            .AddNewRow()
                .AddButton("WithCallbackData", "CallbackData")
                .AddButton(InlineKeyboardButton.WithUrl("WithUrl", "https://github.com/TelegramBots/Telegram.Bot"));
        return await bot.SendMessage(msg.Chat, "Inline buttons:", replyMarkup: inlineMarkup);
    }

    async Task<Message> SendReplyKeyboard(Message msg)
    {
        var replyMarkup = new ReplyKeyboardMarkup(true)
            .AddNewRow("1.1", "1.2", "1.3")
            .AddNewRow().AddButton("2.1").AddButton("2.2");
        return await bot.SendMessage(msg.Chat, "Keyboard buttons:", replyMarkup: replyMarkup);
    }

    async Task<Message> RemoveKeyboard(Message msg)
    {
        return await bot.SendMessage(msg.Chat, "Removing keyboard", replyMarkup: new ReplyKeyboardRemove());
    }

    async Task<Message> RequestContactAndLocation(Message msg)
    {
        var replyMarkup = new ReplyKeyboardMarkup(true)
            .AddButton(KeyboardButton.WithRequestLocation("Location"))
            .AddButton(KeyboardButton.WithRequestContact("Contact"));
        return await bot.SendMessage(msg.Chat, "Who or Where are you?", replyMarkup: replyMarkup);
    }

    async Task<Message> StartInlineQuery(Message msg)
    {
        var button = InlineKeyboardButton.WithSwitchInlineQueryCurrentChat("Inline Mode");
        return await bot.SendMessage(msg.Chat, "Press the button to start Inline Query\n\n" +
            "(Make sure you enabled Inline Mode in @BotFather)", replyMarkup: new InlineKeyboardMarkup(button));
    }

    async Task<Message> SendPoll(Message msg)
    {
        return await bot.SendPoll(msg.Chat, "Question", PollOptions, isAnonymous: false);
    }

    async Task<Message> SendAnonymousPoll(Message msg)
    {
        return await bot.SendPoll(chatId: msg.Chat, "Question", PollOptions);
    }

    static Task<Message> FailingHandler(Message msg)
    {
        throw new NotImplementedException("FailingHandler");
    }

    // Process Inline Keyboard callback data
    private async Task OnCallbackQuery(CallbackQuery callbackQuery, CancellationToken cancellationToken)
    {
        logger.LogInformation("Received inline keyboard callback: {CallbackData}", callbackQuery.Data);

        switch (callbackQuery.Data)
        {
            case "notifications-list":
                await HandleListNotifications(callbackQuery.From.Id, callbackQuery.Message!, cancellationToken);
                break;
            case "notifications-delete":
                await HandleDeleteNotifications(callbackQuery.From.Id, callbackQuery.Message!.Chat, cancellationToken);
                break;
            case "notifications-create":
                await bot.SendMessage(callbackQuery.Message!.Chat, "Создание уведомлений пока не реализовано.");
                break;
            case "notifications-update":
                await bot.SendMessage(callbackQuery.Message!.Chat, "Обновление уведомлений пока не реализовано.");
                break;
            default:
                await bot.SendMessage(callbackQuery.Message!.Chat, $"Получена команда: {callbackQuery.Data}", cancellationToken: cancellationToken);
                break;
        }

        await bot.AnswerCallbackQuery(callbackQuery.Id);
    }

    #region Inline Mode

    private async Task OnInlineQuery(InlineQuery inlineQuery)
    {
        logger.LogInformation("Received inline query from: {InlineQueryFromId}", inlineQuery.From.Id);

        InlineQueryResult[] results = [ // displayed result
            new InlineQueryResultArticle("1", "Telegram.Bot", new InputTextMessageContent("hello")),
            new InlineQueryResultArticle("2", "is the best", new InputTextMessageContent("world"))
        ];
        await bot.AnswerInlineQuery(inlineQuery.Id, results, cacheTime: 0, isPersonal: true);
    }

    private async Task OnChosenInlineResult(ChosenInlineResult chosenInlineResult)
    {
        logger.LogInformation("Received inline result: {ChosenInlineResultId}", chosenInlineResult.ResultId);
        await bot.SendMessage(chosenInlineResult.From.Id, $"You chose result with Id: {chosenInlineResult.ResultId}");
    }

    #endregion

    private Task OnPoll(Poll poll)
    {
        logger.LogInformation("Received Poll info: {Question}", poll.Question);
        return Task.CompletedTask;
    }

    private async Task OnPollAnswer(PollAnswer pollAnswer)
    {
        var answer = pollAnswer.OptionIds.FirstOrDefault();
        var selectedOption = PollOptions[answer];
        if (pollAnswer.User != null)
            await bot.SendMessage(pollAnswer.User.Id, $"You've chosen: {selectedOption.Text} in poll");
    }

    private Task UnknownUpdateHandlerAsync(Update update)
    {
        logger.LogInformation("Unknown update type: {UpdateType}", update.Type);
        return Task.CompletedTask;
    }

    private async Task<Message> HandleGreetings(Message msg, CancellationToken cancellationToken)
    {
        var user = msg.From;
        
        var username = user?.Username;
        var fullName = $"{user?.FirstName} {user?.LastName}".Trim();

        var userDto = new TelegramUserDto
        {
            Name = fullName,
            TelegramTag = username ?? "@unknown",
            TelegramUserId = msg.From!.Id
        };
        
        await mediator.Publish(new UserCreatedDomainEvent(userDto), cancellationToken);

        logger.LogInformation("User joined: {UserId} | @{Username} | {FullName}", msg.From!.Id, username, fullName);

        var greetingText = $"""
            👋 <b>Привет, {fullName}!</b>

            Я — бот, который поможет тебе получать уведомления и работать с системой.

            Нажми на кнопку ниже, чтобы начать 🚀
            """;

        var keyboard = new ReplyKeyboardMarkup(
            new KeyboardButton("📨 Получить уведомления"), 
            new KeyboardButton("ℹ️ Помощь"))
        {
            ResizeKeyboard = true
        };

        return await bot.SendMessage(
            msg.Chat,
            greetingText,
            parseMode: ParseMode.Html,
            replyMarkup: keyboard,
            cancellationToken: cancellationToken
        );
    }
    
    private async Task<Message> SendHelp(Message msg)
    {
        const string helpText = """
            ℹ️ <b>Помощь</b>
    
            Я могу:
            • Присылать уведомления
            • Отвечать на команды
            • Показывать кнопки и меню
    
            Используй /notifications чтобы протестировать уведомления.
            """;

        return await bot.SendMessage(msg.Chat, helpText, parseMode: ParseMode.Html);
    }

    private async Task<Message> SendNotifications(Message msg)
    {
        const string notificationText = """
            📨 <b>Уведомления</b>

            Здесь ты можешь управлять уведомлениями:

            🆕 Создать — добавить новое уведомление  
            🗑️ Удалить — удалить существующее  
            ✏️ Обновить — изменить уведомление  
            📋 Список — посмотреть все уведомления  
            """;

        var keyboard = new InlineKeyboardMarkup([
            [
                InlineKeyboardButton.WithCallbackData("🆕 Создать", "notifications-create"),
                InlineKeyboardButton.WithCallbackData("🗑️ Удалить", "notifications-delete")
            ],
            [
                InlineKeyboardButton.WithCallbackData("✏️ Обновить", "notifications-update"),
                InlineKeyboardButton.WithCallbackData("📋 Список", "notifications-list")
            ]
        ]);

        return await bot.SendMessage(
            chatId: msg.Chat,
            text: notificationText,
            parseMode: ParseMode.Html,
            replyMarkup: keyboard);
    }
    
    private async Task HandleListNotifications(long telegramUserId, Message msg, CancellationToken cancellationToken)
    {
        var notifications = await mediator.Send(new GetUserNotificationsQuery
        {
            TelegramUserId = telegramUserId
        }, cancellationToken);

        if (!notifications.Any())
        {
            await bot.SendMessage(msg.Chat, "У тебя пока нет уведомлений.", cancellationToken: cancellationToken);
            return;
        }

        var textBlocks = notifications.Select(n => 
            $"""
             <b>📋 {n.Title}</b>
             Описание: {n.Description}
             Тип: {n.Type}
             {(!string.IsNullOrEmpty(n.Frequency) ? $"Периодичность: {n.Frequency}" : "")}
             Запланировано на: {n.ScheduledAt:dd.MM.yyyy HH:mm}
             """);

        var text = string.Join("\n\n", textBlocks);

        await bot.SendMessage(msg.Chat, text, parseMode: ParseMode.Html, cancellationToken: cancellationToken);
    }
    
    private async Task HandleDeleteNotifications(long telegramUserId, Chat chat, CancellationToken cancellationToken)
    {
        var notifications = await mediator.Send(new GetUserNotificationsQuery
        {
            TelegramUserId = telegramUserId
        }, cancellationToken);

        if (!notifications.Any())
        {
            await bot.SendMessage(chat, "У тебя нет уведомлений для удаления.", cancellationToken: cancellationToken);
            return;
        }

        var listText = string.Join("\n", notifications
            .Select((n, index) => $"{index + 1}. {n.Title} ({n.ScheduledAt:dd.MM.yyyy})"));

        _pendingNotificationDeletions[telegramUserId] = notifications.Select(n => n.Id).ToList();

        await bot.SendMessage(chat, 
            $"📋 Вот твои уведомления:\n\n{listText}\n\n" +
            "Введите номер уведомления, которое хотите удалить.", 
            cancellationToken: cancellationToken);
    }
}