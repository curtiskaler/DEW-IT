using System.Reflection;

namespace DewIt.Model.DataTypes;

// TODO: Ensure that reflection works on Mac, etc.
// TODO: Serialization?

/// <summary> A set of named constants whose underlying type is any integral type. </summary>
public abstract class Enumeration<E> : IComparable, IComparable<E>, IComparable<Enumeration<E>> where E : Enumeration<E>
{
    /// <summary> The numeric identifier of the current instance. </summary>
    public int Ordinal { get; init; }

    /// <summary> The string name of the current instance. </summary>
    public string Name { get; init; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Enumeration{E}"/> class.
    /// </summary>
    /// <param name="ordinal">The numeric identifier of the instance.</param>
    /// <param name="name">The string name of the instance.</param>
    protected Enumeration(int ordinal, string name)
    {
        Ordinal = ordinal;
        Name = name;
    }

    /// <summary>Returns a set of values that represents the current <see cref="Enumeration{E}"/>.</summary>
    /// <returns>A set of values that represents the current <see cref="Enumeration{E}"/>.</returns>
    public static IEnumerable<T> GetAll<T>() where T : Enumeration<T> =>
        typeof(T).GetFields(BindingFlags.Public |
                            BindingFlags.Static |
                            BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();

    /// <summary>
    /// Parses the <see cref="Enumeration{E}"/> of the given type, and returns the instance with the given <paramref name="name"/>.
    /// </summary>
    /// <typeparam name="T">The <see cref="Enumeration{E}"/> type to search.</typeparam>
    /// <param name="name">The name of the instance to return.</param>
    /// <returns>The instance of the given <see cref="Enumeration{E}"/> with the specified <paramref name="name"/></returns>
    public static T FromName<T>(string name) where T : Enumeration<T>
    {
        var matchingItem = Parse<T, string>(name, "display name", item => item.Name == name);
        return matchingItem;
    }

    /// <summary>
    /// Parses the <see cref="Enumeration{E}"/> of the given type, and returns the instance with the given <paramref name="ordinal"/>.
    /// </summary>
    /// <typeparam name="T">The Enumeration type to search.</typeparam>
    /// <param name="ordinal">The ordinal value of the instance to return.</param>
    /// <returns>The instance of the given <see cref="Enumeration{E}"/> with the specified <paramref name="ordinal"/></returns>
    public static T FromOrdinal<T>(int ordinal) where T : Enumeration<T>
    {
        var matchingItem = Parse<T, int>(ordinal, "ordinal", item => item.Ordinal == ordinal);
        return matchingItem;
    }

    private static T Parse<T, K>(K value, string description, Func<T, bool> predicate) where T : Enumeration<T>
    {
        var matchingItem = GetAll<T>().FirstOrDefault(predicate);

        if (matchingItem == null)
            throw new InvalidOperationException($"'{value}' is not a valid {description} in {typeof(T)}");

        return matchingItem;
    }

    /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
    /// <param name="obj">An object to compare with this instance.</param>
    /// <exception cref="T:System.ArgumentException">
    /// <paramref name="obj" /> is not the same type as this instance.</exception>
    /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="obj" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="obj" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="obj" /> in the sort order.</description></item></list></returns>
    int IComparable.CompareTo(object? obj)
    {
        if (obj is not Enumeration<E> other)
        {
            throw new ArgumentException("Cannot compare to null.");
        }

        return Ordinal.CompareTo(other.Ordinal);
    }

    /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
    public int CompareTo(Enumeration<E>? other)
    {
        if (ReferenceEquals(this, other)) return 0;
        if (other is null) return 1;
        var idComparison = Ordinal.CompareTo(other.Ordinal);
        if (idComparison != 0) return idComparison;
        return string.Compare(Name, other.Name, StringComparison.Ordinal);
    }

    /// <summary>Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.</summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns>A value that indicates the relative order of the objects being compared. The return value has these meanings:
    /// <list type="table"><listheader><term> Value</term><description> Meaning</description></listheader><item><term> Less than zero</term><description> This instance precedes <paramref name="other" /> in the sort order.</description></item><item><term> Zero</term><description> This instance occurs in the same position in the sort order as <paramref name="other" />.</description></item><item><term> Greater than zero</term><description> This instance follows <paramref name="other" /> in the sort order.</description></item></list></returns>
    public int CompareTo(E? other)
    {
        return (other is null) ? 1 : CompareTo((Enumeration<E>)other);
    }

    /// <summary>Determines whether the specified object has the same ordinal of the current <see cref="Enumeration{E}"/>.</summary>
    /// <param name="obj">The object to compare with the current <see cref="Enumeration{E}"/>.</param>
    /// <returns>
    /// <see langword="true" /> if the specified object is equal to the current <see cref="Enumeration{E}"/>; otherwise, <see langword="false" />.</returns>
    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration<E> otherValue)
        {
            return false;
        }

        var typeMatches = GetType() == obj.GetType();
        var valueMatches = Ordinal.Equals(otherValue.Ordinal);

        return typeMatches && valueMatches;
    }

    /// <summary>Determines whether the ordinal of the specified <see cref="Enumeration{E}"/> is equal to that of the current <see cref="Enumeration{E}"/>.</summary>
    /// <param name="obj">The <see cref="Enumeration{E}"/> to compare with the current <see cref="Enumeration{E}"/>.</param>
    /// <returns>
    /// <see langword="true" /> if the ordinal of the specified <see cref="Enumeration{E}"/> is equal to the current <see cref="Enumeration{E}"/>; otherwise, <see langword="false" />.</returns>
    public bool Equals(Enumeration<E> obj)
    {
        return Equals((object?)obj);
    }

    /// <summary>Serves as the default hash function.</summary>
    /// <returns>A hash code for the current object.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(Ordinal);
    }

    /// <summary>Returns a string that represents the current object.</summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => Name;
}