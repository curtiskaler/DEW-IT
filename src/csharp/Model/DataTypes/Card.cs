using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DewIt.Model.DataTypes;

[Table("Cards")]
public sealed class Card
{
    [PrimaryKey, AutoIncrement]
    public Guid UUID { get; init; } = Guid.NewGuid();
    public string DisplayName { get; set; } = "";
    public string Description { get; set; } = "";
    public int Order { get; set; } = 0;

    // TODO: Uncomment and add DB typing info
    //public IUser Owner { get; set; } = IUser.UNSPECIFIED;
    //public Priority Priority { get; set; } = Priority.NORMAL;
    //public ICategory Category { get; set; } = ICategory.UNSPECIFIED;
    //public IHistoryEntry[] History { get; set; } = new List<IHistoryEntry>().ToArray();
    //public ILabel[] Labels { get; set; } = new List<ILabel>().ToArray();
    //public IComment[] Comments { get; set; } = new List<IComment>().ToArray();
    
    public int Version { get; set; } = 0;

    //[ForeignKey(typeof(Group))]
    //public Guid GroupUUID { get; set; }
    //[ManyToOne]
    //public Group? Group { get; set; }


    [ForeignKey(typeof(Lane))]
    public Guid LaneUUID { get; set; }

    //TODO: make non-nullable
    [ManyToOne]
    public Lane? Lane { get; set; }
}
