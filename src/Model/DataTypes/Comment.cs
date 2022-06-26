namespace DewIt.Model.DataTypes
{
    public interface IComment : IUnique, IHaveOwner, IHaveOrder, IHaveHistory
    {
        string Text { get; set; }
    }

    public class Comment : IComment
    {
        public Guid UUID { get; init; } = Guid.NewGuid();
        public IUser Owner { get; set; } = IUser.UNSPECIFIED;
        public int Order { get; set; } = 0;
        public IHistoryEntry[] History { get; set; } = new List<IHistoryEntry>().ToArray();
        public string Text { get; set; } = "";
    }
}