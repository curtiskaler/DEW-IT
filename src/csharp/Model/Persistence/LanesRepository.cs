using DewIt.Model.DataTypes;

namespace DewIt.Model.Persistence;

public interface ILaneRepository : IRepository<Lane>
{
}

public class LanesRepository : Repository<Lane>, ILaneRepository
{
    public LanesRepository(IResource resource) : base(resource)
    {
    }

    public override Task<List<Lane>> GetAll()
    {
        var lanes = Database.Table<Lane>().ToList();
        var cards = Database.Table<Card>().ToList();
        var result = lanes.GroupJoin(cards, column => column.UUID, card => card.LaneUUID,
            (column, columnCards) =>
            {
                column.Cards = columnCards.ToObservableCollection();
                return column;
            }).ToList();
        return Task.FromResult(result);
    }

    public override Task CascadeDelete(Lane lane)
    {
        Database.Delete(lane);
        return Task.CompletedTask;
    }
}