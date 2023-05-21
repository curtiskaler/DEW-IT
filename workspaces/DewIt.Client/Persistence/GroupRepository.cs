using B7.Persistence;
using DewIt.Client.Model;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Persistence;

public interface IGroupRepository : IRepository<Group> { }

public class GroupRepository : SQLiteRepository<Group>, IGroupRepository
{
    public GroupRepository(ILogger logger, IResource resource) : base(logger, resource, DewItRepos.GROUPS)
    {
    }
}