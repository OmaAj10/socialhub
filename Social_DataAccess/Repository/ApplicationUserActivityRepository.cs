using Social_DataAccess.Repository.IRepository;
using Social_Models;

namespace Social_DataAccess.Repository;

public class ApplicationUserActivityRepository : Repository<ApplicationUserActivity>, IApplicationUserActivityRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ApplicationUserActivityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Update(ApplicationUserActivity obj)
    {
        _dbContext.ApplicationUserActivities.Update(obj);
    }
}