using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassR.Application.Abstractions
{
    /// <summary>
    /// Defines a middleware component in the PassR pipeline that can wrap request handling logic.
    /// 
    /// <para>
    /// Pipeline behaviors are useful for implementing cross-cutting concerns such as logging,
    /// validation, caching, transaction management, etc.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">The type of request being handled.</typeparam>
    /// <typeparam name="TResponse">The type of response returned by the handler.</typeparam>
    public interface IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Handles the given request and optionally executes logic before or after the next step in the pipeline.
        /// </summary>
        /// <param name="request">The incoming request object.</param>
        /// <param name="cancellationToken">A token to observe while waiting for the task to complete.</param>
        /// <param name="next">The delegate representing the next step in the pipeline (usually the request handler).</param>
        /// <returns>A task that returns the response from the pipeline.</returns>
        ValueTask<TResponse> HandleAsync(
            TRequest request, 
            CancellationToken cancellationToken, 
            RequestHandlerDelegate<TResponse> next);
    }

    /// <summary>
    /// Represents the next delegate in the request handling pipeline.
    /// This is typically the actual handler that processes the request.
    /// </summary>
    /// <typeparam name="TResponse">The type of the response returned.</typeparam>
    /// <returns>A task that returns the response.</returns>
    public delegate ValueTask<TResponse> RequestHandlerDelegate<TResponse>();
}
