using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Social_DataAccess.Repository.IRepository;

namespace Social_DataAccess.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly ApplicationDbContext _dbContext;
    internal DbSet<T> dbSet;

    public Repository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        this.dbSet = _dbContext.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null)
    {
        IQueryable<T> query = dbSet;
        
        if (filter != null)
        {
            query = query.Where(filter);
        }
        
        if (!string.IsNullOrEmpty(includeProperties))
        {
            foreach (var includeProp in includeProperties.Split(new char[] { ',' },
                         StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProp);
            }
        }

        return await query.ToListAsync();
    }

    public async Task<T> Get(Expression<Func<T, bool>> filter)
    {
        IQueryable<T> query = dbSet;
        query = query.Where(filter);

        return await query.FirstOrDefaultAsync();
    }

    public async Task<T> Create(T entity)
    {
        await dbSet.AddAsync(entity);
       
        return entity;
    }

    public async Task<bool> Delete(T entity)
    {
        dbSet.Remove(entity);
        var result = await _dbContext.SaveChangesAsync();
        
        return result > 0;
    }
}