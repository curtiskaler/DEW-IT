namespace DewIt.Model.DataTypes
{

    public interface IProject : 
        IUnique, IHaveDisplayName, IHaveDescription, 
        IHaveOwner, IHaveIcon, IContainer<IBoard>, 
        IHaveUsers
    {
    }
}