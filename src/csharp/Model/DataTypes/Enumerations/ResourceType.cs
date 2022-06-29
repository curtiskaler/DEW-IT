namespace DewIt.Model.DataTypes;

/// <summary> Types of DewIt resources. </summary>
public abstract class ResourceType : Enumeration<ResourceType>
{
    public static readonly ResourceType NOT_SPECIFIED = new Instance(0, nameof(NOT_SPECIFIED));
    public static readonly ResourceType CARD = new Instance(1, nameof(CARD));
    public static readonly ResourceType GROUP = new Instance(2, nameof(GROUP));
    public static readonly ResourceType LANE = new Instance(3, nameof(LANE));
    public static readonly ResourceType BOARD = new Instance(4, nameof(BOARD));
    public static readonly ResourceType PROJECT = new Instance(5, nameof(PROJECT));

    protected ResourceType(int id, string name) : base(id, name)
    {
    }

    private class Instance : ResourceType
    {
        internal Instance(int id, string name) : base(id, name)
        {
        }
    }
}