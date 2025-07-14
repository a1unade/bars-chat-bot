using NotifyHub.Application.Interfaces;
using NotifyHub.Application.Interfaces.Repositories;

namespace NotifyHub.Persistence.Repositories;

public class UserRepository(IDbContext context) : IUserRepository
{
    private readonly IDbContext _context = context;
    
    // TODO реализовать CRUD-взаимодействие с бд через IDbContext и EntityFramework
}