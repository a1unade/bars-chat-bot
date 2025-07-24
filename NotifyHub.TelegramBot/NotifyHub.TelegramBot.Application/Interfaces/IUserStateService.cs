using NotifyHub.TelegramBot.Domain.Common.Enums;

namespace NotifyHub.TelegramBot.Application.Interfaces;

public interface IUserStateService
{
    UserState GetState(long userId);
    
    void SetState(long userId, UserState state);
    
    void ClearState(long userId);
}