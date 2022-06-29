namespace DewIt.Model.DataTypes;

/// <summary> Types of audit entries. </summary>
public abstract class HistoryEntryType : Enumeration<HistoryEntryType>
{
    public static readonly HistoryEntryType NOT_SPECIFIED = new Instance(0, nameof(NOT_SPECIFIED));
    public static readonly HistoryEntryType ACCESSED = new Instance(1, nameof(ACCESSED));
    public static readonly HistoryEntryType CREATED = new Instance(2, nameof(CREATED));
    public static readonly HistoryEntryType DELETED = new Instance(3, nameof(DELETED));
    public static readonly HistoryEntryType UPDATED = new Instance(4, nameof(UPDATED));
    public static readonly HistoryEntryType ADDED_CHILD = new Instance(5, nameof(ADDED_CHILD));
    public static readonly HistoryEntryType REMOVED_CHILD = new Instance(6, nameof(REMOVED_CHILD));
    public static readonly HistoryEntryType MOVED = new Instance(7, nameof(MOVED));

    protected HistoryEntryType(int id, string name) : base(id, name)
    {
    }

    private class Instance : HistoryEntryType
    {
        internal Instance(int id, string name) : base(id, name)
        {
        }
    }
}