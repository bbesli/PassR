namespace PassR.Events
{
    /// <summary>
    /// Marker interface representing a notification event in the system.
    ///
    /// <para>
    /// Notifications are typically used to broadcast domain events or other system-wide
    /// messages that do not expect a response (fire-and-forget).
    /// </para>
    /// 
    /// <para>
    /// Handlers implementing <see cref="PassR.Application.Abstractions.INotificationHandler{TNotification}"/> 
    /// can react to these notifications asynchronously.
    /// </para>
    /// </summary>
    public interface INotification { }
}
