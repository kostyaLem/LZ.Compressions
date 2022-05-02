using System.Collections.Generic;

namespace LZ.Compressions.Core.Algorithms
{
    public interface ITextCompressor
    {
        bool Validate(string input);
        string Compress(string uncompressed);
        string Decompress(string compressed);
    }
}