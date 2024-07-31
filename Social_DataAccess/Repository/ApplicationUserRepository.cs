using Social_DataAccess.Repository.IRepository;
using Social_Models;

namespace Social_DataAccess.Repository.Repository;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Update(ApplicationUser obj)
    {
        _dbContext.ApplicationUsers.Update(obj);
    }
}