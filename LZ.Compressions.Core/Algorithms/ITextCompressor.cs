using LZ.Compressions.Core.Models;

namespace LZ.Compressions.Core.Algorithms
{
    public interface ITextCompressor : IComperssorValidator<string>
    {
        CompressResult Compress(string uncompressed);
        string Decompress(string compressed);
    }
}