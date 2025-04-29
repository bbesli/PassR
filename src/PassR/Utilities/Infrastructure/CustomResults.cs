using Microsoft.AspNetCore.Http;
using PassR.Abstractions;
using PassR.Shared;

namespace PassR.Utilities.Infrastructure
{
    /// <summary>
    /// Provides a utility method for converting a <see cref="Result"/> failure into a standardized HTTP problem response.
    /// </summary>
    public static class CustomResults
    {
        /// <summary>
        /// Converts a failed <see cref="Result"/> into an <see cref="IResult"/> that represents a standardized RFC 7807 problem response.
        /// 
        /// <para>
        /// The response includes appropriate HTTP status codes, titles, types, and optional validation error details
        /// based on the <see cref="ErrorType"/> of the result.
        /// </para>
        /// </summary>
        /// <param name="result">The result to convert. Must be a failed result.</param>
        /// <returns>An <see cref="IResult"/> that can be returned from a minimal API endpoint.</returns>
        /// <exception cref="InvalidOperationException">Thrown if the result is successful.</exception>
        public static IResult Problem(Result result)
        {
            if (result.IsSuccess)
            {
                throw new InvalidOperationException();
            }

            return Results.Problem(
                title: GetTitle(result.Error),
                detail: GetDetail(result.Error),
                type: GetType(result.Error.Type),
                statusCode: GetStatusCode(result.Error.Type),
                extensions: GetErrors(result));

            static string GetTitle(Error error) =>
                error.Type switch
                {
                    ErrorType.Validation => error.Code,
                    ErrorType.Problem => error.Code,
                    ErrorType.NotFound => error.Code,
                    ErrorType.Conflict => error.Code,
                    _ => "Server failure"
                };

            static string GetDetail(Error error) =>
                error.Type switch
                {
                    ErrorType.Validation => error.Description,
                    ErrorType.Problem => error.Description,
                    ErrorType.NotFound => error.Description,
                    ErrorType.Conflict => error.Description,
                    _ => "An unexpected error occurred"
                };

            static string GetType(ErrorType errorType) =>
                errorType switch
                {
                    ErrorType.Validation => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    ErrorType.Problem => "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    ErrorType.NotFound => "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    ErrorType.Conflict => "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    _ => "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                };

            static int GetStatusCode(ErrorType errorType) =>
                errorType switch
                {
                    ErrorType.Validation => StatusCodes.Status400BadRequest,
                    ErrorType.NotFound => StatusCodes.Status404NotFound,
                    ErrorType.Conflict => StatusCodes.Status409Conflict,
                    _ => StatusCodes.Status500InternalServerError
                };

            static Dictionary<string, object?>? GetErrors(Result result)
            {
                if (result.Error is not ValidationError validationError)
                {
                    return null;
                }

                return new Dictionary<string, object?>
                {
                    { "errors", validationError.Errors }
                };
            }
        }
    }
}
