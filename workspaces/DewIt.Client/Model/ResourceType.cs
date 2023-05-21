using B7.Identifiers;
using B7.Types;

namespace DewIt.Client.Model;

/// <summary> Types of DewIt resources. </summary>
public abstract class ResourceType : Enumeration<ResourceType>
{
    public static ResourceType NOT_SPECIFIED => new Instance(0, nameof(NOT_SPECIFIED));
    public static ResourceType CARD => new Instance(1, nameof(CARD));
    public static ResourceType GROUP => new Instance(2, nameof(GROUP));
    public static ResourceType LANE => new Instance(3, nameof(LANE));
    public static ResourceType BOARD => new Instance(4, nameof(BOARD));
    public static ResourceType PROJECT => new Instance(5, nameof(PROJECT));

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
