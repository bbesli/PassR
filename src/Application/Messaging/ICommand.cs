using PassR.Application.Abstractions;
using PassR.Domain.Abstractions;

namespace PassR.Application.Messaging;

/// <summary>
/// Represents a command that performs an operation and returns a <see cref="Result"/>.
/// Commands typically modify state or trigger side effects.
/// </summary>
public interface ICommand : IRequest<Result>, IBaseCommand
{
}

/// <summary>
/// Represents a command that performs an operation and returns a <see cref="Result{TResponse}"/>.
/// Use this when the command needs to return data.
/// </summary>
public interface ICommand<TReponse> : IRequest<Result<TReponse>>, IBaseCommand
{
}

/// <summary>
/// Marker interface to group command-related abstractions.
/// </summary>
public interface IBaseCommand
{
}
