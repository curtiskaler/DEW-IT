namespace B7.Events;

/// <summary>
/// Defines an interface to get instances of an event type.
/// </summary>
public interface IEventAggregator
{
    /// <summary>
    /// Gets an instance of an event type.
    /// </summary>
    /// <typeparam name="TEventType">The type of event to get.</typeparam>
    /// <returns>An instance of an event object of type <typeparamref name="TEventType"/>.</returns>
    TEventType GetEvent<TEventType>() where TEventType : EventBase, new();

}

/// <summary>
/// Implementation of <see cref="IEventAggregator"/>.
/// </summary>
public class EventAggregator : IEventAggregator
{
    private readonly Dictionary<Type, EventBase> events = new();

    // Captures the sync context for tthe UI thread when constructed on the UI
    // thread in a platform-agnostic way, so that it can be used for UI thread
    // dispatching.
    private readonly SynchronizationContext synchronizationContext = SynchronizationContext.Current;

    /// <summary>
    /// Gets the single instance of the event managed by this EventAggregator.
    /// Multiple calls to this method with the same <typeparamref name="TEventType"/>
    /// returns the same instance.
    /// </summary>
    /// <typeparam name="TEventType">The type of event to get. This must inherit
    /// from <see cref="EventBase"/>.</typeparam>
    /// <returns>A singleton instance of an event object of type
    /// <typeparamref name="TEventType"/>.</returns>
    public TEventType GetEvent<TEventType>() where TEventType : EventBase, new()
    {
        lock (events)
        {
            if (!events.TryGetValue(typeof(TEventType), out var existingEvent))
            {
                var newEvent = new TEventType { SynchronizationContext = synchronizationContext };
                events[typeof(TEventType)] = newEvent;
                return newEvent;
            }
            else
            {
                return (TEventType)existingEvent;
            }
        }
    }
}

