using Social_DataAccess.Repository.Repository;

namespace Social_DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    IActivityRepository Activity { get; }
    IApplicationUserRepository ApplicationUser { get; }
    Task Save();
}