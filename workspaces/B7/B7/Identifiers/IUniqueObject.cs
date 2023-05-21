namespace B7.Identifiers;

public interface IUniqueObject<TIdentifier> where TIdentifier : IEquatable<TIdentifier>
{
    TIdentifier ID { get; init; }
}

public interface IUniqueObject : IUniqueObject<Guid>
{
}