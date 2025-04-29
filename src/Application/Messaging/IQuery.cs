using Application.Abstractions;
using Domain.Abstractions;

namespace Application.Messaging;

public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}
