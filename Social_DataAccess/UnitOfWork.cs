using Social_DataAccess.Repository.IRepository;

namespace Social_DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public IActivityRepository Activity { get; private set; }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        Activity = new ActivityRepository(_dbContext);
    }
    
    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}