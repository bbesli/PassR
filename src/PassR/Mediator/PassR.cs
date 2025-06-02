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
        public async ValueTask<TResponse> SendAsync<TResponse>(
            IRequest<TResponse> request,
            CancellationToken cancellationToken = default)
        {
            var requestType = request.GetType();
            var responseType = typeof(TResponse);

            // Resolve the concrete handler type
            var handlerType = typeof(IRequestHandler<,>).MakeGenericType(requestType, responseType);
            var handler = _serviceProvider.GetRequiredService(handlerType);

            RequestHandlerDelegate<TResponse> handlerDelegate = async () =>
            {
                var method = handlerType.GetMethod("HandleAsync");
                if (method is null)
                    throw new InvalidOperationException($"Method 'HandleAsync' not found on handler '{handler.GetType().Name}'.");

                var task = (ValueTask<TResponse>)method.Invoke(handler, new object[] { request, cancellationToken })!;
                return await task;
            };

            var pipelineType = typeof(IPipelineBehavior<,>).MakeGenericType(requestType, responseType);
            var behaviors = _serviceProvider.GetServices(pipelineType).Reverse().ToList();

            foreach (var behavior in behaviors)
            {
                var method = pipelineType.GetMethod("HandleAsync");
                var next = handlerDelegate;

                handlerDelegate = async () =>
                {
                    var result = method!.Invoke(behavior, new object[] { request, next, cancellationToken });
                    return await (ValueTask<TResponse>)result!;
                };
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
