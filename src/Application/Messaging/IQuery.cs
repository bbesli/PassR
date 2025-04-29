using PassR.Application.Abstractions;
using PassR.Domain.Abstractions;

namespace PassR.Application.Messaging;

/// <summary>
/// Represents a read-only request that returns a result of type <typeparamref name="TResponse"/>.
/// Queries are used to retrieve data and must not change application state.
/// </summary>
/// <typeparam name="TResponse">The type of data returned in the result.</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
