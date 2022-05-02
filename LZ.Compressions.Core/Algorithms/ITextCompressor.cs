namespace LZ.Compressions.Core.Algorithms
{
    public interface ITextCompressor : IComperssorValidator<string>
    {
        string Compress(string uncompressed);
        string Decompress(string compressed);
    }
}