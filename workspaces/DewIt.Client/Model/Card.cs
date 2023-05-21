using B7.Identifiers;
using DewIt.Client.Persistence;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DewIt.Client.Model;

[Table(DewItRepos.CARDS)]
public sealed class Card : IUniqueObject
{
    [PrimaryKey, AutoIncrement]
    public Guid ID { get; init; } = Guid.NewGuid();
    public string Description { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public int Order { get; set; } = 0;

    // TODO: Uncomment and add DB typing info
    //public IUser Owner { get; set; } = IUser.UNSPECIFIED;
    //public List<IHistoryEntry> History { get; set; } = new List<IHistoryEntry>();
    //public Priority Priority { get; set; } = Priority.NORMAL;
    //public ICategory Category { get; set; } = ICategory.UNSPECIFIED;
    //public List<ILabel> Labels { get; set; } = new List<ILabel>();
    //public List<IComment> Comments { get; set; } = new List<IComment>();

    public int Version { get; set; } = 0;

    [ForeignKey(typeof(Lane))]
    public Guid LaneID { get; set; }

    // TODO: Make non-nullable
    [ManyToOne]
    public Lane? Lane { get; set; } = null;
}

public static class ICardExtensions
{
    public static void Move(this Card card, DewItURN target)
    {

    }
}
