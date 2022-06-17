namespace DewIt.Client.model.enumerations
{
    public interface IHaveHistory
    {
        IHistoryEntry[] History { get; set; }
    }
}