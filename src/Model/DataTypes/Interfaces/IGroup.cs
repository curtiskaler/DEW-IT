namespace DewIt.Model.DataTypes
{
    public interface IGroup : IUnique, IHaveDisplayName, IHaveDescription, IHaveOrder, IHaveOwner, IHaveColor, IContainer<ICard>, IHaveGenealogy
    {
    }
}