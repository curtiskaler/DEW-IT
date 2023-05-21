namespace DewIt.Client.Model;

public interface IHaveHistory
{
    List<IHistoryEntry> History { get; set; }
}
