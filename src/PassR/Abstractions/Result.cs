using System.Diagnostics.CodeAnalysis;

namespace PassR.Abstractions
{
    /// <summary>
    /// Represents the outcome of an operation, indicating success or failure, along with an associated error if any.
    /// 
    /// <para>
    /// This class is used to encapsulate the result of commands or queries without throwing exceptions.
    /// It supports fluent error handling and a clear success/failure API surface.
    /// </para>
    /// </summary>
    public class Result
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Result"/> class.
        /// </summary>
        /// <param name="isSuccess">Indicates whether the operation was successful.</param>
        /// <param name="error">The error associated with the result. Must be <see cref="Error.None"/> if successful.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown if the success/error combination is invalid (e.g., success with an error or failure without an error).
        /// </exception>
        public Result(bool isSuccess, Error error)
        {
            if (isSuccess && error != Error.None)
            {
                throw new InvalidOperationException();
            }

            if (!isSuccess && error == Error.None)
            {
                throw new InvalidOperationException();
            }

            IsSuccess = isSuccess;
            Error = error;
        }

        /// <summary>
        /// Gets a value indicating whether the result represents success.
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets a value indicating whether the result represents failure.
        /// </summary>
        public bool IsFailure => !IsSuccess;

        /// <summary>
        /// Gets the associated error, if any.
        /// </summary>
        public Error Error { get; }

        /// <summary>
        /// Creates a successful <see cref="Result"/>.
        /// </summary>
        public static Result Success() => new(true, Error.None);

        /// <summary>
        /// Creates a failed <see cref="Result"/> with a specified error.
        /// </summary>
        /// <param name="error">The error associated with the failure.</param>
        /// <returns>A failed result.</returns>
        public static Result Failure(Error error) => new(false, error);

        /// <summary>
        /// Creates a successful <see cref="Result{TValue}"/> containing a value.
        /// </summary>
        /// <typeparam name="TValue">The type of the success value.</typeparam>
        /// <param name="value">The value to return.</param>
        /// <returns>A successful result containing the value.</returns>
        public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);

        /// <summary>
        /// Creates a failed <see cref="Result{TValue}"/> with a specified error.
        /// </summary>
        /// <typeparam name="TValue">The type of the expected success value.</typeparam>
        /// <param name="error">The error associated with the failure.</param>
        /// <returns>A failed result.</returns>
        public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);

        /// <summary>
        /// Creates a result based on the presence of a non-null value.
        /// Returns a success if value is not null; otherwise, returns a failure with a <see cref="Error.NullValue"/>.
        /// </summary>
        /// <typeparam name="TValue">The type of value.</typeparam>
        /// <param name="value">The value to evaluate.</param>
        /// <returns>A success or failure result.</returns>
        public static Result<TValue> Create<TValue>(TValue? value) =>
            value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    }

    /// <summary>
    /// Represents the outcome of an operation that produces a value upon success.
    /// </summary>
    /// <typeparam name="TValue">The type of the value returned if successful.</typeparam>
    public sealed class Result<TValue> : Result
    {
        private readonly TValue? _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="Result{TValue}"/> class.
        /// </summary>
        /// <param name="value">The success value.</param>
        /// <param name="isSuccess">Indicates whether the operation was successful.</param>
        /// <param name="error">The error associated with the result.</param>
        public Result(TValue? value, bool isSuccess, Error error)
            : base(isSuccess, error)
        {
            _value = value;
        }

        /// <summary>
        /// Gets the value associated with a successful result.
        /// Throws an exception if accessed on a failed result.
        /// </summary>
        [NotNull]
        public TValue Value => IsSuccess
            ? _value!
            : throw new InvalidOperationException("The value of a failure result cannot be accessed.");

        /// <summary>
        /// Implicitly creates a successful <see cref="Result{TValue}"/> from a non-null value.
        /// </summary>
        /// <param name="value">The value to wrap as a result.</param>
        public static implicit operator Result<TValue>(TValue? value) => Create(value);
    }
}
