namespace DewIt.Client.model.enumerations
{
    public interface IGroup : IUnique, IHaveDisplayName, IHaveDescription, IHaveOrder, IHaveOwner, IHaveColor, IContainer<ICard>, IHaveGenealogy
    {
    }
}