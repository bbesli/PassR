using PassR.Application.Abstractions;
using PassR.Domain.Abstractions;

namespace PassR.Application.Messaging;

/// <summary>
/// Handles a query and returns a <see cref="Result{TResponse}"/>.
/// Queries are used to retrieve data from the system.
/// </summary>
/// <typeparam name="TQuery">The type of query being handled.</typeparam>
/// <typeparam name="TResponse">The type of the result value returned.</typeparam>
public interface IQueryHandler<TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}
