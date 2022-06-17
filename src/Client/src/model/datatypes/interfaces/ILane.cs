namespace DewIt.Client.model.enumerations
{
    public interface ILane : IUnique, IHaveDisplayName, IHaveDescription, IHaveOwner, IHaveOrder, IHaveColor, IContainer<IGroup>, IHaveGenealogy
    {
    }
}