namespace DewIt.Model.DataTypes
{
    public interface IContainer<T> where T : IUnique
    {
        T[] Items { get; set; }
    }
}