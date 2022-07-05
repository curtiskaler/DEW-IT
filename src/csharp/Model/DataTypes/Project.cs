using System.Collections.ObjectModel;

namespace DewIt.Model.DataTypes
{
    public class Project 
    {
        public Guid UUID { get; init; } = Guid.NewGuid();
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public IUser Owner { get; set; } = IUser.UNSPECIFIED;
        public ICollection<Board> Boards { get; set; }

        public Project()
        {
            Boards = new ObservableCollection<Board>();
        }
    }
}