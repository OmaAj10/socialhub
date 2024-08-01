using Social_DataAccess.Repository.IRepository;
using Social_DataAccess.Repository.Repository;

namespace Social_DataAccess.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext;
    public IActivityRepository Activity { get; private set; }
    public IApplicationUserRepository ApplicationUser { get; private set; }
    public IApplicationUserActivityRepository ApplicationUserActivity { get; private set; }

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        Activity = new ActivityRepository(_dbContext);
        ApplicationUser = new ApplicationUserRepository(_dbContext);
        ApplicationUserActivity = new ApplicationUserActivityRepository(_dbContext);
    }
    
    public async Task Save()
    {
        await _dbContext.SaveChangesAsync();
    }
}