namespace DewIt.Model.DataTypes
{
    public interface IComment : IUnique, IHaveOwner, IHaveOrder, IHaveHistory, IHaveGenealogy
    {
        String Text { get; set; }
    }
}