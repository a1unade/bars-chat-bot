using AppAny.HotChocolate.FluentValidation;
using AutoMapper;
using NotifyHub.Domain.Entities;
using NotifyHub.Application.Requests.User;
using NotifyHub.Application.Interfaces.Repositories;
using NotifyHub.Application.Validators;
using NotifyHub.Domain.DTOs;

namespace NotifyHub.Application.Features.Mutations;

[ExtendObjectType("Mutation")]
public class UserMutation(IMapper mapper): BaseMutation
{
    [GraphQLDescription("Создание пользователя")]
    public async Task<Guid> CreateUserAsync(
        [Service] IGenericRepository<User> userRepository,
        [UseFluentValidation, UseValidator<CreateUserRequestValidator>]
        CreateUserRequest request,
        CancellationToken cancellationToken)
    {
        var userEntity = mapper.Map<User>(request);
        var savedUser = await Add(userRepository, userEntity, cancellationToken);

        return savedUser.Id;
    }
    
    [GraphQLDescription("Обновление информации о пользователе")]
    public async Task<UserDto> UpdateUserAsync(
        [Service] IGenericRepository<User> userRepository,
        [UseFluentValidation, UseValidator<UpdateUserRequestValidator>]
        UpdateUserRequest request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);

        if (user is null)
            throw new ArgumentException("Пользователь с указанным ID не найден");
        
        if (request.Name is not null)
            user.Name = request.Name;

        if (request.Email is not null)
            user.Email = request.Email;
        
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