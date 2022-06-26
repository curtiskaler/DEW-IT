namespace DewIt.Model.DataTypes
{
    public class DewItURN : URN
    {
        public static readonly string NamespaceID = "DewIt";

        private DewItURN(string nss) : base($"{URNScheme}:{NamespaceID}:{nss}")
        {
        }

        public DewItURN(ResourceType type, string identifier) : this($"{type}:{identifier}")
        {
        }
    }
}