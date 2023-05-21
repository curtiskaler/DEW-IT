using B7.Identifiers;

namespace DewIt.Client.Model;

public class DewItURN : Urn
{
    public static readonly string NamespaceID = "DewIt";

    private DewItURN(string nss) : base($"{URNScheme}:{NamespaceID}:{nss}")
    {
    }

    public DewItURN(ResourceType type, string identifier) : this($"{type}:{identifier}")
    {
    }
}
