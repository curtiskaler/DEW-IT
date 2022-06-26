namespace DewIt.Model.DataTypes
{
    public interface IBoard : 
        IUnique, IHaveDisplayName, IHaveDescription, 
        IHaveOwner, IHaveIcon, IContainer<Lane>
    {
    }

    public class Board : IBoard
    {
        public Guid UUID { get; init; } = Guid.NewGuid();
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public IUser Owner { get; set; } = IUser.UNSPECIFIED;
        public Lane[] Items { get; set; } = new List<Lane>().ToArray();
    }
}