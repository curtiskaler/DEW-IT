using B7;
using B7.Persistence;
using DewIt.Client.Model;
using Microsoft.Extensions.Logging;

namespace DewIt.Client.Persistence;

public interface ILaneRepository : IRepository<Lane> { }

public class LaneRepository : SQLiteRepository<Lane>, ILaneRepository
{
    public LaneRepository(ILogger logger, IResource resource) : base(logger, resource, DewItRepos.LANES)
    {
    }

    public override Task<List<Lane>> GetAll()
    {
        // TODO: The Database call creates the Lanes table, but we need to also create the Cards table.
        // perhaps an 'ensure initialized' call somewhere?

        var lanes = Database.Table<Lane>().ToList();
        var cards = Database.Table<Card>().ToList();
        var result = lanes.GroupJoin(cards, column => column.ID, card => card.LaneID,
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