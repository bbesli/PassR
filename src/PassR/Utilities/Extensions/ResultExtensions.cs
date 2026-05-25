using PassR.Abstractions;
using Microsoft.AspNetCore.Http;

namespace PassR.Utilities.Extensions
{
    /// <summary>
    /// Provides functional pattern-matching extensions for <see cref="Result"/> and <see cref="Result{T}"/>,
    /// as well as helpers to convert Results to HTTP responses.
    /// </summary>
    public static class ResultExtensions
    {
        /// <summary>
        /// Matches a non-generic <see cref="Result"/>, invoking the appropriate delegate based on success or failure.
        /// </summary>
        /// <typeparam name="TOut">The return type of the match result.</typeparam>
        /// <param name="result">The result to match against.</param>
        /// <param name="onSuccess">The function to execute if the result is successful.</param>
        /// <param name="onFailure">The function to execute if the result has failed.</param>
        /// <returns>The result of the executed delegate.</returns>
        public static TOut Match<TOut>(
            this Result result,
            Func<TOut> onSuccess,
            Func<Result, TOut> onFailure)
        {
            return result.IsSuccess ? onSuccess() : onFailure(result);
        }

        /// <summary>
        /// Matches a generic <see cref="Result{TIn}"/>, invoking the appropriate delegate based on success or failure.
        /// </summary>
        /// <typeparam name="TIn">The type of value in the successful result.</typeparam>
        /// <typeparam name="TOut">The return type of the match result.</typeparam>
        /// <param name="result">The result to match against.</param>
        /// <param name="onSuccess">The function to execute if the result is successful.</param>
        /// <param name="onFailure">The function to execute if the result has failed.</param>
        /// <returns>The result of the executed delegate.</returns>
        public static TOut Match<TIn, TOut>(
            this Result<TIn> result,
            Func<TIn, TOut> onSuccess,
            Func<Result<TIn>, TOut> onFailure)
        {
            return result.IsSuccess ? onSuccess(result.Value) : onFailure(result);
        }

        /// <summary>
        /// Converts a non-generic <see cref="Result"/> to an <see cref="IResult"/> HTTP response.
        /// Returns 200 OK with the serialized Result on success, or 400 BadRequest on failure.
        /// </summary>
        /// <param name="result">The result to convert.</param>
        /// <returns>An <see cref="IResult"/> representing the HTTP response.</returns>
        public static IResult ToHttpResult(this Result result)
        {
            return result.IsSuccess
                ? Results.Ok(result)
                : Results.BadRequest(result);
        }

        /// <summary>
        /// Converts a generic <see cref="Result{TValue}"/> to an <see cref="IResult"/> HTTP response.
        /// Returns 200 OK with the serialized Result on success, or 400 BadRequest on failure.
        /// </summary>
        /// <typeparam name="TValue">The type of the value in the result.</typeparam>
        /// <param name="result">The result to convert.</param>
        /// <returns>An <see cref="IResult"/> representing the HTTP response.</returns>
        public static IResult ToHttpResult<TValue>(this Result<TValue> result)
        {
            return result.IsSuccess
                ? Results.Ok(result)
                : Results.BadRequest(result);
        }
    }
}
