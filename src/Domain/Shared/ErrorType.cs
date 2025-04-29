namespace PassR.Domain.Shared
{
    /// <summary>
    /// Defines the type of error that can occur within the system.
    /// Used to categorize different kinds of operational and validation failures.
    /// </summary>
    public enum ErrorType
    {
        /// <summary>
        /// Represents a general operation failure.
        /// </summary>
        Failure = 0,

        /// <summary>
        /// Represents a validation error, such as invalid user input.
        /// </summary>
        Validation = 1,

        /// <summary>
        /// Represents a general problem or issue not covered by other categories.
        /// </summary>
        Problem = 2,

        /// <summary>
        /// Represents a resource not found error.
        /// </summary>
        NotFound = 3,

        /// <summary>
        /// Represents a conflict, such as a duplicate entry or a versioning issue.
        /// </summary>
        Conflict = 4
    }
}
