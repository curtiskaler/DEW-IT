using DewIt.Model.DataTypes;

namespace DewIt.Model.Persistence;

public interface ICardRepository : IRepository<Card>
{
}

public class CardsRepository : Repository<Card>, ICardRepository
{
    public CardsRepository(IResource resource) : base(resource)
    {
    }
}