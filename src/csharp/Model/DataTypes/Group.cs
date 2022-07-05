using System.Collections.ObjectModel;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DewIt.Model.DataTypes;

[Table("Groups")]
public sealed class Group
{
    [PrimaryKey, AutoIncrement]
    public Guid UUID { get; init; } = Guid.NewGuid();

    [OneToMany(CascadeOperations = CascadeOperation.CascadeDelete)]
    public ICollection<Card> Cards { get; set; }

    public string Color { get; set; } = "#FFFFFFFF"; /* transparent white */
    public string Description { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public int Order { get; set; } = 0;
    //public IUser Owner { get; set; } = IUser.UNSPECIFIED;

    public Group()
    {
        Cards = new ObservableCollection<Card>();
    }
}