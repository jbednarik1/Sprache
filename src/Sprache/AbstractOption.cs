namespace Sprache
{
    internal abstract class AbstractOption<T> : IOption<T>
    {
        public abstract bool IsEmpty { get; }

        public bool IsDefined => !IsEmpty;

        public T GetOrDefault()
        {
            return IsEmpty ? default(T) : Get();
        }

        public abstract T Get();
    }
}