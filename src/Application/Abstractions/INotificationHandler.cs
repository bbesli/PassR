using Domain.Events;

namespace Application.Abstractions
{
    public interface INotificationHandler<TNotification> where TNotification : INotification
    {
        ValueTask HandleAsync(TNotification notification, CancellationToken cancellationToken = default);
    }
}
