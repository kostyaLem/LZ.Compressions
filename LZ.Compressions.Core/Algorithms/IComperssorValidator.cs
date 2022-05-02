namespace LZ.Compressions.Core.Algorithms
{
    public interface IComperssorValidator<T>
    {
        bool ValidateBeforeCompress(T input);
        bool ValidateBeforeDecompress(T input);
    }
}