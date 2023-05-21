using B7.Identifiers;

namespace DewIt.Client.Model;

public interface IComment : IUniqueObject, IHaveOwner, IHaveOrder, IHaveHistory
{
    string Text { get; set; }
}

public class Comment : IComment
{
    public Guid ID { get; init; } = Guid.NewGuid();
    public IUser Owner { get; set; } = IUser.UNSPECIFIED;
    public int Order { get; set; } = 0;
    public List<IHistoryEntry> History { get; set; } = new List<IHistoryEntry>();
    public string Text { get; set; } = "";
}
