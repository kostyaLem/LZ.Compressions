using System.Collections.Generic;

namespace LZ.Compressions.Core.Algorithms
{
    public interface ITextCompressor
    {
        string Compress(string uncompressed);
        string Decompress(string compressed);
    }
}