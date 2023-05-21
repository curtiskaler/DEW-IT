using B7.Identifiers;

namespace DewIt.Client.Model;

public interface ILabel : IUniqueObject, IHaveDisplayName
{
}

public class Label : ILabel
{
    public Guid ID { get; init; } = Guid.NewGuid();
    public string DisplayName { get; set; } = "";
}
