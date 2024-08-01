using Social_Models;

namespace Social_DataAccess.Repository.IRepository;

public interface IApplicationUserActivityRepository : IRepository<ApplicationUserActivity>
{
    Task Update(ApplicationUserActivity obj);
}