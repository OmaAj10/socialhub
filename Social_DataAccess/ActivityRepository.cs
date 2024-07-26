using Social_DataAccess.Repository.IRepository;
using Social_Models;

namespace Social_DataAccess.Repository;

public class ActivityRepository : Repository<Activity>, IActivityRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ActivityRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Update(Activity obj)
    { 
        _dbContext.Activities.Update(obj);
    }
}