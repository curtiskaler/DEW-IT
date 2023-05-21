using System.Collections.ObjectModel;
using B7.Identifiers;
using DewIt.Client.Persistence;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace DewIt.Client.Model;

//public interface IGroup : IUniqueObject, IHaveDisplayName, IHaveDescription, IHaveOrder, IHaveOwner, IHaveColor, IContainer<Card>
//{
//}

[Table(DewItRepos.GROUPS)]
public sealed class Group : IUniqueObject//: IGroup
{
    [PrimaryKey, AutoIncrement]
    public Guid ID { get; init; } = Guid.NewGuid();

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
