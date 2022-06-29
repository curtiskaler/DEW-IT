namespace DewIt.Model.DataTypes
{
    public interface IProject :
        IUnique, IHaveDisplayName, IHaveDescription,
        IHaveOwner, IHaveIcon, IContainer<IBoard>
    {
    }

    public class Project : IProject
    {
        public Guid UUID { get; init; } = Guid.NewGuid();
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public IUser Owner { get; set; } = IUser.UNSPECIFIED;
        public IBoard[] Items { get; set; } = new List<IBoard>().ToArray();
    }
}