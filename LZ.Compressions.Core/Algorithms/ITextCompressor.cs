using LZ.Compressions.Core.Models;
using System.Collections.Generic;

namespace LZ.Compressions.Core.Algorithms
{
    public interface ITextCompressor : IComperssorValidator<string>
    {
        CompressResult Compress(string uncompressed);
        string Decompress(string compressed, IDictionary<int, string> initialDictionary = default);
    }
}