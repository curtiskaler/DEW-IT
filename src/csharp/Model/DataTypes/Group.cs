namespace DewIt.Model.DataTypes
{
    public interface IGroup : IUnique, IHaveDisplayName, IHaveDescription, IHaveOrder, IHaveOwner, IHaveColor,
        IContainer<Card>
    {
    }

    public class Group : IGroup
    {
        public Guid UUID { get; init; } = Guid.NewGuid();
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public int Order { get; set; } = 0;
        public IUser Owner { get; set; } = IUser.UNSPECIFIED;
        public string Color { get; set; } = "#FFFFFFFF"; /* transparent white */
        public Card[] Items { get; set; } = new List<Card>().ToArray();
    }
}