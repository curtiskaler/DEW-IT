using B7.Persistence;
using DewIt.Client.Model;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Persistence;

public interface ICardRepository : IRepository<Card> { }

public class CardRepository : SQLiteRepository<Card>, ICardRepository
{
    public CardRepository(ILogger logger, IResource resource) : base(logger, resource, DewItRepos.CARDS)
    {
    }
}