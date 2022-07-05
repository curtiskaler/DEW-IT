using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace DewIt.Model.DataTypes;

[Table("Lanes")]
public sealed class Lane
{
    [PrimaryKey, AutoIncrement]
    public Guid UUID { get; init; } = Guid.NewGuid();

    [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
    public ICollection<Card> Cards { get; set; }

    public string Color { get; set; } = "#FFFFFFFF"; /* transparent white */
    public string Description { get; set; } = "";
    public string DisplayName { get; set; } = "";

    //[OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
    //public ICollection<Group> Groups { get; set; }

    public int MaxWorkInProgress { get; set; } = int.MaxValue;
    public int Order { get; set; }
    
    
    //public IUser Owner { get; set; } = IUser.UNSPECIFIED;
    
    public Lane()
    {
        //Groups = new ObservableCollection<Group>();
        Cards = new ObservableCollection<Card>();
    }
}

public class LaneInfo
{
    public Lane Lane { get; }
    public int Index { get; }

    public LaneInfo(int index, Lane lane)
    {
        Index = index;
        Lane = lane;
    }

    public bool IsMaxWIPReached => Lane.Cards.Count >= Lane.MaxWorkInProgress;
}