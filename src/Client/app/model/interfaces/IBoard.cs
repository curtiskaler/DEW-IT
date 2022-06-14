namespace DewIt.Client.model.enumerations
{
    public interface IBoard : 
        IUnique, IHaveDisplayName, IHaveDescription, 
        IHaveOwner, IHaveIcon, IContainer<ILane>, 
        IHaveGenealogy, IHaveUsers
    {
    }
}