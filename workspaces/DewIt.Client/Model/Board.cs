using System.Collections.ObjectModel;
using B7.Identifiers;
using DewIt.Client.Persistence;
using SQLite;

namespace DewIt.Client.Model;

//public interface IBoard :
//       IUniqueObject, IHaveDisplayName, IHaveDescription,
//       IHaveOwner, IHaveIcon, IContainer<Lane>
//{
//}

[Table(DewItRepos.BOARDS)]
public class Board : IUniqueObject //: IBoard
{
    public Guid ID { get; init; } = Guid.NewGuid();
    public string DisplayName { get; set; } = "";
    public string Description { get; set; } = "";
    public IUser Owner { get; set; } = IUser.UNSPECIFIED;
    public ICollection<Lane> Lanes { get; set; }

    public Board()
    {
        Lanes = new ObservableCollection<Lane>();
    }
}
