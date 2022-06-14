namespace DewIt.Client.model.enumerations
{

    public interface IProject : 
        IUnique, IHaveDisplayName, IHaveDescription, 
        IHaveOwner, IHaveIcon, IContainer<IBoard>, 
        IHaveUsers
    {
    }
}