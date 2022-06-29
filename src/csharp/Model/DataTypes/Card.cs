namespace DewIt.Model.DataTypes
{
    public interface ICard : IUnique, IHaveDisplayName, IHaveDescription, IHaveOrder, IHaveOwner, IHaveHistory
    {
        Priority Priority { get; set; }

        ICategory Category { get; set; }

        ILabel[] Labels { get; set; }

        IComment[] Comments { get; set; }

        int Version { get; set; }
    }

    public class Card : ICard
    {
        public Guid UUID { get; init; } = Guid.NewGuid();
        public string DisplayName { get; set; } = "";
        public string Description { get; set; } = "";
        public int Order { get; set; } = 0;
        public IUser Owner { get; set; } = IUser.UNSPECIFIED;
        public IHistoryEntry[] History { get; set; } = new List<IHistoryEntry>().ToArray();
        public Priority Priority { get; set; } = Priority.NORMAL;
        public ICategory Category { get; set; } = ICategory.UNSPECIFIED;
        public ILabel[] Labels { get; set; } = new List<ILabel>().ToArray();
        public IComment[] Comments { get; set; } = new List<IComment>().ToArray();
        public int Version { get; set; } = 0;
    }
    
    public static class CardExtensions
    {
        public static void Move(this ICard card, DewItURN target)
        {
            
        }
    }
}