using PassR.Shared;

namespace PassR.Utilities.Exceptions
{
    /// <summary>
    /// Represents an exception that is thrown when one or more validation errors occur during an operation.
    ///
    /// <para>
    /// This exception is typically thrown by application or domain services when input data fails validation rules.
    /// It carries a collection of <see cref="ValidationError"/> instances describing each individual validation failure.
    /// </para>
    /// </summary>
    public sealed class ValidationException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class with the specified validation errors.
        /// </summary>
        /// <param name="errors">A collection of validation errors that caused the exception.</param>
        public ValidationException(IEnumerable<ValidationError> errors)
        {
            Errors = errors;
        }

        /// <summary>
        /// Gets the collection of validation errors associated with this exception.
        /// </summary>
        public IEnumerable<ValidationError> Errors { get; }
    }
}
