using System.Collections.ObjectModel;

namespace DewIt.Model.DataTypes
{
    public class Board 
    {
        public Guid UUID { get; init; } = Guid.NewGuid();
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public IUser Owner { get; set; } = IUser.UNSPECIFIED;
        public ICollection<Lane> Lanes { get; set; }

        public Board()
        {
            Lanes = new ObservableCollection<Lane>();
        }
    }
}