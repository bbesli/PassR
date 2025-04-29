using Microsoft.AspNetCore.Routing;

namespace PassR.Utilities.Endpoints
{
    /// <summary>
    /// Defines a contract for endpoint mapping within an ASP.NET Core application.
    ///
    /// <para>
    /// Implement this interface to modularize route definitions and organize endpoint registration
    /// separately from the main <c>Program.cs</c> or startup files.
    /// </para>
    ///
    /// <para>
    /// Endpoints implementing <see cref="IEndpoint"/> can be discovered and mapped dynamically at runtime
    /// to improve scalability, maintainability, and separation of concerns in large applications.
    /// </para>
    /// </summary>
    public interface IEndpoint
    {
        /// <summary>
        /// Maps the endpoints to the specified <see cref="IEndpointRouteBuilder"/>.
        /// </summary>
        /// <param name="app">The endpoint route builder used to configure routes.</param>
        void MapEndpoint(IEndpointRouteBuilder app);
    }
}
