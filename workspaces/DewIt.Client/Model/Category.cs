using B7.Identifiers;

namespace DewIt.Client.Model;

public interface ICategory : IUniqueObject, IHaveDisplayName, IHaveIcon
{
    public static readonly ICategory UNSPECIFIED = new Category
    {
        ID = Guid.Empty,
        DisplayName = "unspecified"
    };
}

public class Category : ICategory
{
    public Guid ID { get; init; } = Guid.NewGuid();
    public string DisplayName { get; set; } = "";
}
