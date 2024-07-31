using Social_Models;

namespace Social_DataAccess.Repository.IRepository;

public interface IApplicationUserRepository : IRepository<ApplicationUser>
{
    Task Update(ApplicationUser obj);
}