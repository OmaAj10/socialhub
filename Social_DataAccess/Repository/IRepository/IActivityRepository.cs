using Social_Models;

namespace Social_DataAccess.Repository.IRepository;

public interface IActivityRepository : IRepository<Activity>
{
    Task Update(Activity obj);
}