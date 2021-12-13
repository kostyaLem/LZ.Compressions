namespace LZ.Compressions.Core.Algorithms
{
    public interface IReadableCompressor
    {
        string GetReadableView(string compressed);
    }
}
