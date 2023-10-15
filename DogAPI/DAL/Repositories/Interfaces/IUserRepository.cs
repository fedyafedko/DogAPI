using DAL.Repositories.Base;
using Entities;

namespace DAL.Repositories.Interfaces;

public interface IUserRepository : IRepo<User, Guid>
{
    User? FindByLogin(string login);
    Task<User?> FindByLoginAsync(string login);
}