using System.Collections.ObjectModel;

namespace DewIt.Client.Model;

//public interface IProject :
//    IUniqueObject, IHaveDisplayName, IHaveDescription,
//    IHaveOwner, IHaveIcon, IContainer<IBoard>
//{
//}

public class Project //: IProject
{
    public Guid ID { get; init; } = Guid.NewGuid();
    public string Description { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public IUser Owner { get; set; } = IUser.UNSPECIFIED;
    public ICollection<Board> Boards { get; set; }

    public Project()
    {
        Boards = new ObservableCollection<Board>();
    }
}