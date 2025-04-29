using PassR.Domain.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassR.Application.Abstractions
{
    /// <summary>
    /// Defines the core mediator interface for the PassR framework, enabling
    /// decoupled communication between request senders and their corresponding handlers.
    /// 
    /// <para>
    /// It supports both <c>SendAsync</c> for request/response operations (queries or commands)
    /// and <c>PublishAsync</c> for fire-and-forget notifications (events).
    /// </para>
    /// </summary>
    public interface IPassR
    {
        /// <summary>
        /// Sends a request to the appropriate handler and returns a response.
        /// This is typically used for commands and queries that expect a result.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response expected from the handler.</typeparam>
        /// <param name="request">The request object implementing <see cref="IRequest{TResponse}"/>.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task that resolves to the handler's response.</returns>
        ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);

        /// <summary>
        /// Publishes a notification to all registered handlers.
        /// Notifications are one-way, fire-and-forget events that do not return a result.
        /// </summary>
        /// <typeparam name="TNotification">The type of the notification, which must implement <see cref="INotification"/>.</typeparam>
        /// <param name="notification">The notification instance to publish.</param>
        /// <param name="cancellationToken">Optional cancellation token.</param>
        /// <returns>A task that completes when all handlers have finished processing.</returns>
        ValueTask PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default) where TNotification : INotification;
    }
}
