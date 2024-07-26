namespace Social_DataAccess.Repository.IRepository;

public interface IUnitOfWork
{
    IActivityRepository Activity { get; }
    Task Save();
}