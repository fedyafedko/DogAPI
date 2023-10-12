using DAL.EF;
using Microsoft.EntityFrameworkCore;

namespace DAL.Repositories.Base;


public class Repo<TEntity, TKey> : IRepo<TEntity, TKey> 
    where TEntity : class
    where TKey : IEquatable<TKey>
{
    private ApplicationDbContext _applicationDbContext;
    public DbSet<TEntity> Table { get; }

    public Repo(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
        Table = _applicationDbContext.Set<TEntity>();
    }
    
    public int Add(TEntity entity, bool persist = true)
    {
        Table.Add(entity);
        return persist ? SaveChanges() : 0;
    }

    public async Task<int> AddAsync(TEntity entity, bool persist = true)
    {
        await Table.AddAsync(entity);
        return persist ? await SaveChangesAsync() : 0;
        
    }

    public TEntity? Find(TKey key)
    {
        return Table.Find(key);
    }

    public async Task<TEntity?> FindAsync(TKey key)
    {
        return await Table.FindAsync(key);
    }

    public int AddRange(IEnumerable<TEntity> entities, bool persist = true)
    {
        Table.AddRange(entities);
        return persist ? SaveChanges() : 0;
    }

    public async Task<int> AddRangeAsync(IEnumerable<TEntity> entities, bool persist = true)
    {
        await Table.AddRangeAsync(entities);
        return persist ? await SaveChangesAsync() : 0;
    }

    public int Update(TEntity entity, bool persist = true)
    {
        Table.Update(entity);
        return persist ? SaveChanges() : 0;
    }

    public async Task<int> UpdateAsync(TEntity entity, bool persist = true)
    {
        Table.Update(entity);
        return persist ? await SaveChangesAsync() : 0;
    }

    public int Delete(TEntity entity, bool persist = true)
    {
        Table.Remove(entity);
        return persist ? SaveChanges() : 0;
    }

    public async Task<int> DeleteAsync(TEntity entity, bool persist = true)
    {
        Table.Remove(entity);
        return persist ? await SaveChangesAsync() : 0;
    }

    public IEnumerable<TEntity> GetAll()
    {
        return Table;
    }

    public int SaveChanges()
    {
        try
        {
            return _applicationDbContext.SaveChanges();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred updating the database", ex);
        }
    }

    public async Task<int> SaveChangesAsync()
    {
        try
        {
            return await _applicationDbContext.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred updating the database", ex);
        }
    }
}