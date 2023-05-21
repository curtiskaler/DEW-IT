using B7.Identifiers;

namespace DewIt.Client.Model;

public interface IContainer<T> where T : IUniqueObject
{
    List<T> Items { get; set; }
}