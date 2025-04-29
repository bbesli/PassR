namespace PassR.Common
{
    /// <summary>
    /// Represents a void-like return type for requests that do not produce a result.
    /// 
    /// <para>
    /// Use <c>Unit</c> as the response type for commands or operations that are handled
    /// but do not return any meaningful value.
    /// </para>
    /// </summary>
    public readonly struct Unit
    {
        /// <summary>
        /// A static instance of <see cref="Unit"/> used as the canonical return value.
        /// </summary>
        public static readonly Unit Value = new();
    }
}
