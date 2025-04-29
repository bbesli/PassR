using PassR.Domain.Abstractions;

namespace PassR.Domain.Shared
{
    /// <summary>
    /// Represents a collection of validation errors that occurred during an operation.
    /// 
    /// <para>
    /// This specialized error type aggregates multiple individual <see cref="Error"/> instances,
    /// typically resulting from failed validations across different fields or rules.
    /// </para>
    /// </summary>
    public sealed record ValidationError : Error
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationError"/> class with the specified validation errors.
        /// </summary>
        /// <param name="errors">An array of individual validation <see cref="Error"/>s.</param>
        public ValidationError(Error[] errors)
            : base(
                "Validation.General",
                "One or more validation errors occurred",
                ErrorType.Validation)
        {
            Errors = errors;
        }

        /// <summary>
        /// Gets the collection of individual validation errors.
        /// </summary>
        public Error[] Errors { get; }

        /// <summary>
        /// Creates a <see cref="ValidationError"/> from a collection of <see cref="Result"/>s.
        /// Only failed results are considered, and their associated errors are aggregated.
        /// </summary>
        /// <param name="results">The collection of results to extract validation errors from.</param>
        /// <returns>A <see cref="ValidationError"/> containing the extracted errors.</returns>
        public static ValidationError FromResults(IEnumerable<Result> results) =>
            new(results.Where(r => r.IsFailure).Select(r => r.Error).ToArray());
    }
}
