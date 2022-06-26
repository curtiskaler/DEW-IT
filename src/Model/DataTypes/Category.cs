namespace DewIt.Model.DataTypes
{
    public interface ICategory : IUnique, IHaveDisplayName, IHaveIcon
    {
        public static readonly ICategory UNSPECIFIED = new Category
        {
            UUID = Guid.Empty,
            DisplayName = "unspecified"
        };
    }

    public class Category : ICategory
    {
        public Guid UUID { get; init; } = Guid.NewGuid();
        public string DisplayName { get; set; } = "";
    }
}