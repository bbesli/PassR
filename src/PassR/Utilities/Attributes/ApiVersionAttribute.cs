namespace PassR.Utilities.Attributes;

/// <summary>
/// Specifies the API version that an endpoint class belongs to.
/// </summary>
/// <remarks>
/// This attribute can be applied to classes implementing <c>IEndpoint</c>
/// to indicate the version of the API they are part of. Multiple versions
/// can be assigned using multiple instances of this attribute.
/// </remarks>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ApiVersionAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ApiVersionAttribute"/> class.
    /// </summary>
    /// <param name="version">The version number of the API (e.g., 1, 2, 3).</param>
    public ApiVersionAttribute(int version)
    {
        Version = version;
    }

    /// <summary>
    /// Gets the API version number.
    /// </summary>
    public int Version { get; }
}