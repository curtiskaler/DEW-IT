using System.Diagnostics.CodeAnalysis;

namespace B7.Events;

/// <summary>
/// Subscription token returned from <see cref="EventBase"/> on subscribe.
/// </summary>
public class SubscriptionToken : IEquatable<SubscriptionToken>, IDisposable
{
    //#nullable enable
    private readonly Guid _token;
    private Action<SubscriptionToken>? _unsubscribeAction;

    /// <summary>
    /// Initializes a new instance of <see cref="SubscriptionToken"/>.
    /// </summary>
    /// <param name="unsubscribeAction"></param>
    public SubscriptionToken(Action<SubscriptionToken> unsubscribeAction)
    {
        _unsubscribeAction = unsubscribeAction;
        _token = Guid.NewGuid();
    }

    /// <summary>
    /// Determines whether the specified <see cref="SubscriptionToken"/> is equal
    /// to the current instance.
    /// </summary>
    /// <param name="obj">The <see cref="SubscriptionToken"/> to compare with the current instance.</param>
    /// <returns><see langword="true"/> if the specified <see cref="SubscriptionToken"/> is equal to the current instance; otherwise, <see langword="false"/>.</returns>
    public bool Equals(SubscriptionToken? other)
    {
        if (other == null) return false;
        return Equals(_token, other._token);
    }

    /// <summary>
    /// Determines whether the specified <see cref="T:System.Object"/> is equal
    /// to the current <see cref="SubscriptionToken"/>.
    /// </summary>
    /// <param name="obj">The <see cref="T:System.Object"/> to compare with the current <see cref="SubscriptionToken"/>.</param>
    /// <returns><see langword="true"/> if the specified <see cref="T:System.Object"/> is equal to the current <see cref="SubscriptionToken"/>; otherwise, <see langword="false"/>.</returns>
    public override bool Equals(object? obj)
    {
        if (obj == null) return false;
        if (ReferenceEquals(this, obj)) return true;
        return Equals(obj as SubscriptionToken);
    }
#nullable disable

    /// <summary>
    /// Serves as a hash function for a particular type.
    /// </summary>
    /// <returns>A hash code for the current <see cref="SubscriptionToken"/>.</returns>
    public override int GetHashCode()
    {
        return _token.GetHashCode();
    }

    /// <summary>
    /// Disposes the SubscriptionToken, removing the subscriptions from the corresponding <see cref="EventBase"/>.
    /// </summary>
    public void Dispose()
    {
        // While the SubscriptionToken class implements IDisposable,
        // in the case of weak subscriptions (i.e., keepSubscriberReferenceAlive
        // set to false in the Subscribe method), it's not necessary to
        // unsubscribe, as no resources should be kept alive by the event
        // subscription.  In such cases, if a warning is issued, it should be
        // suppressed.

        if (this._unsubscribeAction != null)
        {
            this._unsubscribeAction(this);
            this._unsubscribeAction = null;
        }

        GC.SuppressFinalize(this);
    }
}
