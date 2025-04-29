using PassR.Shared;

namespace PassR.Abstractions
{
    /// <summary>
    /// Represents a structured error within the domain or application layers.
    /// Errors encapsulate a code, a human-readable description, and a type
    /// indicating the category of error.
    /// 
    /// <para>
    /// Static factory methods are provided to easily create common error types,
    /// such as <c>Failure</c>, <c>NotFound</c>, <c>Problem</c>, and <c>Conflict</c>.
    /// </para>
    /// </summary>
    public record Error
    {
        /// <summary>
        /// Represents the absence of an error.
        /// </summary>
        public static readonly Error None = new(string.Empty, string.Empty, ErrorType.Failure);

        /// <summary>
        /// Represents an error indicating that a start date is earlier than an end date.
        /// </summary>
        public static Error StartDateCannotBeLowerThenEndDate => Problem(
            "General.StartDateEndDateRangeError",
            "Start date cannot be lower then end date.");

        /// <summary>
        /// Represents an error indicating that a null value was provided.
        /// </summary>
        public static readonly Error NullValue = new(
            "General.Null",
            "Null value was provided",
            ErrorType.Failure);

        /// <summary>
        /// Initializes a new instance of the <see cref="Error"/> record.
        /// </summary>
        /// <param name="code">A unique code identifying the error.</param>
        /// <param name="description">A human-readable description of the error.</param>
        /// <param name="type">The <see cref="ErrorType"/> categorizing the error.</param>
        public Error(string code, string description, ErrorType type)
        {
            Code = code;
            Description = description;
            Type = type;
        }

        /// <summary>
        /// Gets the error code.
        /// </summary>
        public string Code { get; }

        /// <summary>
        /// Gets the error description.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the error type.
        /// </summary>
        public ErrorType Type { get; }

        /// <summary>
        /// Creates a failure error.
        /// </summary>
        /// <param name="code">The failure code.</param>
        /// <param name="description">The failure description.</param>
        /// <returns>An <see cref="Error"/> instance representing a failure.</returns>
        public static Error Failure(string code, string description) =>
            new(code, description, ErrorType.Failure);

        /// <summary>
        /// Creates a not found error.
        /// </summary>
        /// <param name="code">The not found code.</param>
        /// <param name="description">The not found description.</param>
        /// <returns>An <see cref="Error"/> instance representing a not found error.</returns>
        public static Error NotFound(string code, string description) =>
            new(code, description, ErrorType.NotFound);

        /// <summary>
        /// Creates a general problem error.
        /// </summary>
        /// <param name="code">The problem code.</param>
        /// <param name="description">The problem description.</param>
        /// <returns>An <see cref="Error"/> instance representing a problem.</returns>
        public static Error Problem(string code, string description) =>
            new(code, description, ErrorType.Problem);

        /// <summary>
        /// Creates a conflict error.
        /// </summary>
        /// <param name="code">The conflict code.</param>
        /// <param name="description">The conflict description.</param>
        /// <returns>An <see cref="Error"/> instance representing a conflict.</returns>
        public static Error Conflict(string code, string description) =>
            new(code, description, ErrorType.Conflict);
    }
}
