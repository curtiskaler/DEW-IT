using B7.Identifiers;
using B7.Types;

namespace DewIt.Client.Model;

/// <summary> Types of audit entries. </summary>
public abstract class HistoryEntryType : Enumeration<HistoryEntryType>
{
    public static HistoryEntryType NOT_SPECIFIED => new Instance(0, nameof(NOT_SPECIFIED));
    public static HistoryEntryType ACCESSED => new Instance(1, nameof(ACCESSED));
    public static HistoryEntryType CREATED => new Instance(2, nameof(CREATED));
    public static HistoryEntryType DELETED => new Instance(3, nameof(DELETED));
    public static HistoryEntryType UPDATED => new Instance(4, nameof(UPDATED));
    public static HistoryEntryType ADDED_CHILD => new Instance(5, nameof(ADDED_CHILD));
    public static HistoryEntryType REMOVED_CHILD => new Instance(6, nameof(REMOVED_CHILD));
    public static HistoryEntryType MOVED => new Instance(7, nameof(MOVED));

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
