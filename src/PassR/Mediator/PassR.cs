using Microsoft.Extensions.DependencyInjection;
using PassR.Abstractions;
using PassR.Events;

namespace PassR.Mediator
{
    /// <summary>
    /// Default implementation of the <see cref="IPassR"/> interface.
    /// 
    /// <para>
    /// PassR mediates communication between request senders and their corresponding handlers.
    /// It supports request/response operations (<c>SendAsync</c>) and fire-and-forget notifications (<c>PublishAsync</c>),
    /// with optional pipeline behaviors for cross-cutting concerns such as logging, validation, and caching.
    /// </para>
    /// </summary>
    public class PassR : IPassR
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="PassR"/> class with the specified service provider.
        /// </summary>
        /// <param name="serviceProvider">The service provider used to resolve handlers and pipeline behaviors.</param>
        public PassR(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public async ValueTask<TResponse> SendAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(request.GetType(), typeof(TResponse));
            var handler = _serviceProvider.GetRequiredService(handlerType);

            var behaviors = _serviceProvider
                .GetServices(typeof(IPipelineBehavior<,>).MakeGenericType(request.GetType(), typeof(TResponse)))
                .Cast<dynamic>().Reverse().ToList();

            RequestHandlerDelegate<TResponse> handlerDelegate = () => ((dynamic)handler).HandleAsync((dynamic)request, cancellationToken);

            foreach (var behavior in behaviors)
            {
                var next = handlerDelegate;
                handlerDelegate = () => behavior.HandleAsync((dynamic)request, cancellationToken, next);
            }

            return await handlerDelegate();
        }

        /// <inheritdoc />
        public async ValueTask PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();
            var tasks = handlers.Select(handler => handler.HandleAsync(notification, cancellationToken).AsTask());
            await Task.WhenAll(tasks);
        }
    }
}
