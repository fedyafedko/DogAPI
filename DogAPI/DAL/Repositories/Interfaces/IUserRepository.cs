using DAL.Repositories.Base;
using Entities;

namespace DAL.Repositories.Interfaces;

public interface IUserRepository : IRepo<User, int>
{
    User? FindByLogin(string login);
    Task<User?> FindByLoginAsync(string login);
}