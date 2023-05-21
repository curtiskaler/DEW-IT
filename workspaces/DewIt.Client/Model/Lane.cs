using B7.Identifiers;
using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.ObjectModel;

namespace DewIt.Client.Model;

[Table("Lanes")]
public sealed class Lane : IUniqueObject
{
    [PrimaryKey, AutoIncrement]
    public Guid ID { get; init; } = Guid.NewGuid();

    [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
    public ICollection<Card> Cards { get; set; }

    public string Color { get; set; } = "#FFFFFFFF"; /* transparent white */
    public string Description { get; set; } = "";
    public string DisplayName { get; set; } = "";

    //    [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
    //    public ICollection<Group> Groups { get; set; }

    public int MaxWorkInProgress { get; set; } = int.MaxValue;
    public int Order { get; set; } = 0;

    //public IUser Owner { get; set; } = IUser.UNSPECIFIED;

    public Lane()
    {
        Cards = new ObservableCollection<Card>();
        //Groups = new ObservableCollection<Group>();
    }
}
