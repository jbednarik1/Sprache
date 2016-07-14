namespace Sprache
{
    /// <summary>
    ///     Represents an optional result.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    public interface IOption<out T>
    {
        /// <summary>
        ///     Gets a value indicating whether this instance is empty.
        /// </summary>
        bool IsEmpty { get; }

        /// <summary>
        ///     Gets a value indicating whether this instance is defined.
        /// </summary>
        bool IsDefined { get; }

        /// <summary>
        ///     Gets the matched result or a default value.
        /// </summary>
        /// <returns></returns>
        T GetOrDefault();

        /// <summary>
        ///     Gets the matched result.
        /// </summary>
        T Get();
    }
}