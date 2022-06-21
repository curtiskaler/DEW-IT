namespace DewIt.Model.DataTypes
{
    public interface ILane : IUnique, IHaveDisplayName, IHaveDescription, IHaveOwner, IHaveOrder, IHaveColor, IContainer<IGroup>, IHaveGenealogy
    {
    }
}