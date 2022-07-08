using DewIt.Model.DataTypes;
using Microsoft.Extensions.Logging;

namespace DewIt.Model.Persistence;

public interface ICardRepository : IRepository<Card>
{
}

public class CardsRepository : Repository<Card>, ICardRepository
{
    public CardsRepository(ILogger logger, IResource resource) : base(logger, resource)
    {
    }
}