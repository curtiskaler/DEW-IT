namespace DewIt.Model.DataTypes
{
    public interface IBoard : 
        IUnique, IHaveDisplayName, IHaveDescription, 
        IHaveOwner, IHaveIcon, IContainer<ILane>, 
        IHaveGenealogy, IHaveUsers
    {
    }
}