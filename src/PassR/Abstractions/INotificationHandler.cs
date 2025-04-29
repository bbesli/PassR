using PassR.Events;

namespace PassR.Abstractions
{
    /// <summary>
    /// Defines a handler for processing notifications that implement <see cref="INotification"/>.
    /// Notifications represent fire-and-forget messages that do not return a value.
    /// These are typically used to represent domain events or system-wide broadcasts.
    /// </summary>
    /// <typeparam name="TNotification">
    /// The type of the notification being handled, which must implement <see cref="INotification"/>.
    /// </typeparam>
    /// <example>
    /// Example implementation:
    /// <code>
    /// public class UserRegisteredHandler : INotificationHandler&lt;UserRegistered&gt;
    /// {
    ///     public ValueTask HandleAsync(UserRegistered notification, CancellationToken cancellationToken)
    ///     {
    ///         Console.WriteLine($&quot;User registered: {notification.Email}&quot;);
    ///         return ValueTask.CompletedTask;
    ///     }
    /// }
    /// </code>
    /// </example>
    public interface INotificationHandler<TNotification> where TNotification : INotification
    {
        ValueTask HandleAsync(TNotification notification, CancellationToken cancellationToken = default);
    }
}
