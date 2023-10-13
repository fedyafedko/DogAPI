using DAL.EF;
using DAL.Repositories.Base;
using DAL.Repositories.Interfaces;
using Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories;

public class UserRepository : Repo<User, int>, IUserRepository
{
    public UserRepository(ApplicationDbContext applicationDbContext) : base(applicationDbContext) { }
    
    public User? FindByLogin(string login)
    {
        return Table.FirstOrDefault(user => user.Login == login);
    }
    
    public async Task<User?> FindByLoginAsync(string login)
    {
        return await Table.FirstOrDefaultAsync(user => user.Login == login);
    }
}