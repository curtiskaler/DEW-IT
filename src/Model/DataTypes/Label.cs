namespace DewIt.Model.DataTypes
{
    public interface ILabel : IUnique, IHaveDisplayName
    {
    }

    public class Label : ILabel
    {
        public Guid UUID { get; init; } = Guid.NewGuid();
        public string DisplayName { get; set; } = "";
    }
}