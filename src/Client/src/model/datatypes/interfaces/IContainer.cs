namespace DewIt.Client.model.enumerations
{
    public interface IContainer<T> where T : IUnique
    {
        T[] Items { get; set; }
    }
}