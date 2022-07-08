using DewIt.Model.DataTypes;
using Microsoft.Extensions.Logging;

namespace DewIt.Model.Persistence;

public interface ILaneRepository : IRepository<Lane>
{
}

public class LanesRepository : Repository<Lane>, ILaneRepository
{
    public LanesRepository(ILogger logger, IResource resource) : base(logger, resource)
    {
    }

    public override Task<List<Lane>> GetAll()
    {
        // TODO: The Database call creates the Lanes table, but we need to also create the Cards table.
        // perhaps an 'ensure initialized' call somewhere?

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