namespace PassR.Utilities.Exceptions;

/// <summary>
/// Represents a single field-level validation error with a property name and error message.
/// </summary>
/// <param name="PropertyName">The name of the property that failed validation.</param>
/// <param name="ErrorMessage">A human-readable description of the validation failure.</param>
public sealed record ValidationError(string PropertyName, string ErrorMessage);
