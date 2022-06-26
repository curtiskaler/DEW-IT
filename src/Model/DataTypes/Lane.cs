namespace DewIt.Model.DataTypes
{
    public interface ILane : IUnique, IHaveDisplayName, IHaveDescription, IHaveOwner, IHaveOrder, IHaveColor,
        IContainer<IGroup>
    {
    }

    public class Lane : ILane
    {
        public Guid UUID { get; init; } = Guid.NewGuid();
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public IUser Owner { get; set; } = IUser.UNSPECIFIED;
        public int Order { get; set; } = 0;
        public string Color { get; set; } = "#FFFFFFFF"; /* transparent white */
        public IGroup[] Items { get; set; } = new List<IGroup>().ToArray();
    }
}