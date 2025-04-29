using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PassR.Application.Abstractions
{
    /// <summary>
    /// Defines a handler for processing a specific <see cref="IRequest{TResponse}"/> and returning a response.
    ///
    /// <para>
    /// Implement this interface to handle application logic for queries and commands.
    /// The <c>HandleAsync</c> method will be invoked by the PassR mediator when a matching request is sent.
    /// </para>
    /// </summary>
    /// <typeparam name="TRequest">
    /// The type of request to handle. Must implement <see cref="IRequest{TResponse}"/>.
    /// </typeparam>
    /// <typeparam name="TResponse">
    /// The type of response returned by the handler.
    /// </typeparam>
    public interface IRequestHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        /// <summary>
        /// Handles the incoming request and returns a response.
        /// </summary>
        /// <param name="request">The request to handle.</param>
        /// <param name="cancellationToken">A token to observe while performing the operation.</param>
        /// <returns>A task that resolves to the response.</returns>
        ValueTask<TResponse> HandleAsync(
            TRequest request, 
            CancellationToken cancellationToken = default);
    }
}
