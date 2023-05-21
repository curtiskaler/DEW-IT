using System.Collections.ObjectModel;

namespace B7;

public static class CollectionExtensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> enumerable)
    {
        return new ObservableCollection<T>(enumerable);
    }
}
