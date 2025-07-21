using AutoMapper;
using NotifyHub.Domain.Entities;
using NotifyHub.Application.Requests.User;
using NotifyHub.Application.Interfaces.Repositories;
using NotifyHub.Domain.DTOs;
using NotifyHub.Application.Validators;
using FluentValidation;

namespace NotifyHub.Application.Features.Mutations;

[ExtendObjectType("Mutation")]
public class UserMutation(IMapper mapper): BaseMutation
{
    [GraphQLDescription("Создание пользователя")]
    public async Task<Guid> CreateUserAsync(
        [Service] IGenericRepository<User> userRepository,
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new CreateUserValidation();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var userEntity = mapper.Map<User>(request);
        var savedUser = await Add(userRepository, userEntity, cancellationToken);

        return savedUser.Id;
    }
    
    [GraphQLDescription("Обновление информации о пользователе")]
    public async Task<UserDto> UpdateUserAsync(
        [Service] IGenericRepository<User> userRepository,
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var validator = new UpdateUserValidation();
        await validator.ValidateAndThrowAsync(request, cancellationToken);

        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
            throw new ArgumentException("Пользователь с указанным ID не найден");
        
        if (request.FirstName is not null)
            user.Name = request.FirstName;

        if (request.LastName is not null)
            user.Name = request.LastName;

        if (request.Email is not null)
            user.Email = request.Email;

        if (request.TelegramTag is  not null) 
            user.TelegramTag = request.TelegramTag;
        
        await userRepository.UpdateAsync(user.Id, user, cancellationToken);
        
        return mapper.Map<UserDto>(user);
    }
    
    [GraphQLDescription("Удаление пользователя по ID")]
    public async Task<bool> DeleteUserAsync(
        [Service] IGenericRepository<User> userRepository,
        Guid id,
        CancellationToken cancellationToken)
    {
        await userRepository.RemoveByIdAsync(id, cancellationToken);

        return true;
    }

}