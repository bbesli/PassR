using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PassR.Utilities.Exceptions;

namespace PassR.Utilities.Middleware
{
    /// <summary>
    /// Middleware for globally handling unhandled exceptions in the application.
    /// 
    /// <para>
    /// Converts known exceptions (such as validation or request conflicts) into
    /// standardized <see cref="ProblemDetails"/> responses with appropriate status codes,
    /// and logs unexpected exceptions as server errors.
    /// </para>
    /// </summary>
    internal sealed class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next middleware in the request pipeline.</param>
        /// <param name="logger">The logger instance used for error logging.</param>
        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Executes the middleware logic for handling exceptions during HTTP request processing.
        /// </summary>
        /// <param name="context">The current HTTP context.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Exception occurred: {Message}", exception.Message);

                var exceptionDetails = GetExceptionDetails(exception);

                var problemDetails = new ProblemDetails
                {
                    Status = exceptionDetails.Status,
                    Type = exceptionDetails.Type,
                    Title = exceptionDetails.Title,
                    Detail = exceptionDetails.Detail,
                    Instance = context.Request.Path
                };

                if (exceptionDetails.Errors is not null)
                {
                    problemDetails.Extensions["errors"] = exceptionDetails.Errors;
                }

                context.Response.StatusCode = exceptionDetails.Status;

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }

        /// <summary>
        /// Maps known exception types to their corresponding HTTP error details.
        /// </summary>
        /// <param name="exception">The exception to handle.</param>
        /// <returns>A structured <see cref="ExceptionDetails"/> object representing the error response.</returns>
        private static ExceptionDetails GetExceptionDetails(Exception exception)
        {
            return exception switch
            {
                HttpRequestException => new ExceptionDetails(
                    StatusCodes.Status409Conflict,
                    "Conflict",
                    "Confliction occurred",
                    "ExceptionDetails.Conflict",
                    null),

                ValidationException validationException => new ExceptionDetails(
                    StatusCodes.Status400BadRequest,
                    "ValidationFailure",
                    "Validation error",
                    "Validation.General",
                    validationException.Errors),

                _ => new ExceptionDetails(
                    StatusCodes.Status500InternalServerError,
                    "ServerError",
                    "Server error",
                    "An unexpected error has occurred",
                    null)
            };
        }

        /// <summary>
        /// Represents the structured details used to build a <see cref="ProblemDetails"/> response.
        /// </summary>
        /// <param name="Status">The HTTP status code.</param>
        /// <param name="Type">The machine-readable error type identifier.</param>
        /// <param name="Title">A short, human-readable summary of the problem.</param>
        /// <param name="Detail">A detailed human-readable explanation of the error.</param>
        /// <param name="Errors">An optional list of validation or domain-specific errors.</param>
        internal sealed record ExceptionDetails(
            int Status,
            string Type,
            string Title,
            string Detail,
            IEnumerable<object>? Errors);
    }
}
