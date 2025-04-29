using PassR.Abstractions;

namespace PassR.Utilities.Extensions
{
    /// <summary>
    /// Provides functional pattern-matching extensions for <see cref="Result"/> and <see cref="Result{T}"/>.
    /// 
    /// <para>
    /// These extensions help simplify branching logic by allowing you to express success and failure
    /// outcomes in a concise and readable manner.
    /// </para>
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
    }
}
