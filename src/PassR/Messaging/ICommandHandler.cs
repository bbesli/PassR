using PassR.Abstractions;

namespace PassR.Messaging;

/// <summary>
/// Handles a command that returns a <see cref="Result"/>.
/// Implement this interface for commands that do not return a value.
/// </summary>
/// <typeparam name="TCommand">The type of command being handled.</typeparam>
public interface ICommandHandler<TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

/// <summary>
/// Handles a command that returns a <see cref="Result{TResponse}"/>.
/// Implement this interface for commands that return a value.
/// </summary>
/// <typeparam name="TCommand">The type of command being handled.</typeparam>
/// <typeparam name="TResponse">The type of the result value returned.</typeparam>
public interface ICommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}
