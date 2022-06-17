namespace DewIt.Client.model.enumerations
{
    public interface IComment : IUnique, IHaveOwner, IHaveOrder, IHaveHistory, IHaveGenealogy
    {
        String Text { get; set; }
    }
}