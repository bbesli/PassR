namespace PassR.Abstractions
{
    /// <summary>
    /// Represents a request that expects a response of type <typeparamref name="TResponse"/>.
    /// 
    /// <para>
    /// This interface is the core message contract used to define commands and queries in PassR.
    /// Handlers that process these requests implement <see cref="IRequestHandler{TRequest, TResponse}"/>.
    /// </para>
    /// </summary>
    /// <typeparam name="TResponse">The type of the response expected when the request is handled.</typeparam>
    public interface IRequest<TResponse> { }
}
