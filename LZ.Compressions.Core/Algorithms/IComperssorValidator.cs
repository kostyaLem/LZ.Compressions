namespace LZ.Compressions.Core.Algorithms
{
    public interface IComperssorValidator<T>
    {
        void ValidateBeforeCompress(T input);
        void ValidateBeforeDecompress(T input);
    }
}