using NotifyHub.TelegramBot.Application.Interfaces;
using NotifyHub.TelegramBot.Domain.Common.Enums;
using NotifyHub.TelegramBot.Domain.DTOs;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace NotifyHub.TelegramBot.Infrastructure.Handlers;

public class StartMessageHandler : IMessageHandler
{
    private readonly IUserService _userService;

    public StartMessageHandler(IUserService userService)
    {
        _userService = userService;
    }

    public bool CanHandle(Message msg, UserState state)
    {
        return msg.Text?.Trim().ToLower() == "/start";
    }

    public async Task HandleAsync(Message msg, ITelegramBotClient bot, CancellationToken cancellationToken)
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
        
        await _userService.RegisterUserAsync(userDto, cancellationToken);
        
        var keyboard = new ReplyKeyboardMarkup([
            ["Получить уведомления", "Помощь"]
        ])
        {
            ResizeKeyboard = true,
            OneTimeKeyboard = false
        };

        await bot.SendMessage(
            chatId: msg.Chat.Id,
            text: $"Привет, { fullName }! Ты успешно зарегистрирован.",
            replyMarkup: keyboard,
            cancellationToken: cancellationToken);
    }
}