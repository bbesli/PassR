using Application.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Domain.Events;

namespace Infrastructure.Mediator
{
    public class PassR : IPassR
    {
        private readonly IServiceProvider _serviceProvider;

        public PassR(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

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
                handlerDelegate = () => ((dynamic)behavior).HandleAsync((dynamic)request, cancellationToken, next);
            }

            return await handlerDelegate();
        }

        public async ValueTask PublishAsync<TNotification>(TNotification notification, CancellationToken cancellationToken = default)
            where TNotification : INotification
        {
            var handlers = _serviceProvider.GetServices<INotificationHandler<TNotification>>();
            var tasks = handlers.Select(handler => handler.HandleAsync(notification, cancellationToken).AsTask());
            await Task.WhenAll(tasks);
        }
    }
}
