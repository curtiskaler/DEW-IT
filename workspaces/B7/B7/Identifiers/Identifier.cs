using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;

namespace B7.Identifiers;

/// <summary>
/// Generalized identifier. The primary identifier is the <paramref name="Id"/> property.
/// </summary>
/// <typeparam name="TIdentifier">The datatype of the <paramref name="Id" /> property.</typeparam>
public readonly struct Identifier<TIdentifier> : IEquatable<Identifier<TIdentifier>> where TIdentifier : IEquatable<TIdentifier>
{
    /// <summary>
    /// Gets the identifier for this object.
    /// </summary>
    public TIdentifier Id { get; }

    /// <summary>
    /// Initializes an instance of the <see cref="Identifier"/> struct.
    /// </summary>
    /// <param name="identifier">The identifier to parse.</param>
    public Identifier(TIdentifier identifier)
    {
        Id = identifier;
    }

    /// <summary>
    /// Implicitly creates an <see cref="Identifier{TIdentifier}"/> from the given <typeparamref name="TIdentifier"/>.
    /// </summary>
    /// <param name="id">The <typeparamref name="TIdentifier"/> to convert to an <see cref="Identifier"/>.</param>
    public static implicit operator Identifier<TIdentifier>(TIdentifier id) => new(id);

    /// <summary> Implicitly converts an <see cref="Identifier{TIdentifier}"/> to its corresponding <typeparamref name="TIdentifier"/>. </summary>
    public static implicit operator TIdentifier(Identifier<TIdentifier> identifier) => identifier.Id;

    #region IEquatable

    /// <summary>
    /// Indicates whether the current object is equal to another <see cref="Identifier{TIdentifier}"/>.
    /// Two <see cref="Identifier{TIdentifier}"/>s are equal if they have the same id.
    /// </summary>
    /// <param name="other">A <see cref="Identifier{TIdentifier}"/> to compare with this <see cref="Identifier{TIdentifier}"/>.</param>
    /// <returns><langword>true</langword> if the current object is equal to the other parameter; otherwise, <langword>false</langword>.</returns>
    public bool Equals(Identifier<TIdentifier> other) => other.Id.Equals(Id);

    /// <summary>
    /// Indicates whether the current object is equal to another object.
    /// Two <see cref="Identifier{T}"/>s are equal if they have the same id.
    /// </summary>
    /// <param name="obj">An object to compare with this <see cref="Identifier{TIdentifier}"/>.</param>
    /// <returns><langword>true</langword> if the current object is equal to the other parameter; otherwise, <langword>false</langword>.</returns>
    public override bool Equals([NotNullWhen(true)] object obj) => obj is Identifier<TIdentifier> && Equals(obj);
    public static bool operator ==(Identifier<TIdentifier> x, Identifier<TIdentifier> y) => x.Equals(y);
    public static bool operator !=(Identifier<TIdentifier> x, Identifier<TIdentifier> y) => !(x == y);

    public override int GetHashCode() => base.GetHashCode();

    #endregion

    public override string ToString() => Id.ToString();

}

/// <summary>
/// Generalized identifier, which includes a <paramref name="Name"/>. The primary identifier is the <paramref name="Id"/> property.
/// </summary>
/// <typeparam name="TIdentifier">The datatype of the <paramref name="Id" /> property.</typeparam>
public readonly struct NamedIdentifier<TIdentifier> : IEquatable<NamedIdentifier<TIdentifier>> where TIdentifier : IEquatable<TIdentifier>
{
    /// <summary>
    /// Gets the identifier for this object.
    /// </summary>
    public TIdentifier Id { get; init; }

    /// <summary>
    /// Gets the name of this object.
    /// </summary>
    public string Name { get; init; }

#nullable enable
    /// <summary>
    /// Initializes an instance of the <see cref="NamedIdentifier"/> struct.
    /// </summary>
    /// <param name="identifier"></param>
    /// <param name="name"></param>
    public NamedIdentifier(TIdentifier identifier, string? name = default)
    {
        Id = identifier;
        Name = name;
    }
#nullable disable

    /// <summary>
    /// Initializes an instance of the <see cref="NamedIdentifier"/> struct.
    /// </summary>
    /// <param name="identifier">The <see cref="NamedIdentifier{TIdentifier}"/> to convert to a <see cref="NamedIdentifier{TIdentifier}"/>.</param>
    public NamedIdentifier(NamedIdentifier<TIdentifier> identifier) : this(identifier.Id, identifier.Name)
    {
    }

    /// <summary>
    /// Implicitly converts an <see cref="NamedIdentifier{TIdentifier}"/> to its corresponding <typeparamref name="TIdentifier"/>.
    /// </summary>
    public static implicit operator TIdentifier(NamedIdentifier<TIdentifier> namedIdentifier) => namedIdentifier.Id;

    /// <summary>
    /// Implicitly converts an <see cref="NamedIdentifier{TIdentifier}"/> to its corresponding Name <see cref="string"/>.
    /// </summary>
    public static implicit operator string(NamedIdentifier<TIdentifier> namedIdentifier) => namedIdentifier.Name;

    #region IEquatable

    /// <summary>
    /// Indicates whether the current instance is equal to another <see cref="NamedIdentifier{TIdentifier}"/>. Two named identifiers are equal if they have the same name and id.
    /// </summary>
    /// <param name="other">A <see cref="NamedIdentifier" to compare with this object.</param>
    /// <returns><langword>true</langword> if the current <see cref="NamedIdentifier"/> is equal to the other parameter; otherwise, <langword>false</langword>.</returns>
    public bool Equals(NamedIdentifier<TIdentifier> other) => Id.Equals(other.Id) && Name == other.Name;

    /// <summary>
    /// Indicates whether the current instance is equal to another object. Two named identifiers are equal if they have the same name and id.
    /// </summary>
    /// <param name="other">An object to compare with this instance.</param>
    /// <returns><langword>true</langword> if the current object is equal to the other parameter; otherwise, <langword>false</langword>.</returns>
    public override bool Equals([NotNullWhen(true)] object other) => other is NamedIdentifier<TIdentifier> && Equals(other);
    public static bool operator ==(NamedIdentifier<TIdentifier> x, NamedIdentifier<TIdentifier> y) => x.Equals(y);
    public static bool operator !=(NamedIdentifier<TIdentifier> x, NamedIdentifier<TIdentifier> y) => !(x == y);

    public override int GetHashCode() => (Id, Name).GetHashCode();

    #endregion

    public override string ToString() => $"{Name} :: {Id}";
}
