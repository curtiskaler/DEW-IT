namespace DewIt.Model.DataTypes
{
    public interface IHaveHistory
    {
        IHistoryEntry[] History { get; set; }
    }
}