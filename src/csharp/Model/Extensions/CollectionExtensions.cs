using System.Collections.ObjectModel;

namespace DewIt.Model;

public static class CollectionExtensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> col)
    {
        return new ObservableCollection<T>(col);
    }
}