using B7.Persistence;
using DewIt.Client.Model;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Persistence;

public interface IUserRepository : IRepository<User> { }

public class UserRepository : SQLiteRepository<User>, IUserRepository
{
    public UserRepository(ILogger logger, IResource resource) : base(logger, resource, DewItRepos.USERS)
    {
    }
}