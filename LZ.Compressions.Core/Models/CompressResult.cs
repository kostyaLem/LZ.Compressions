using System.Collections.Generic;

namespace LZ.Compressions.Core.Models
{
    public record CompressResult(string CompressedText, int CompressedLength, IReadOnlyList<string> Dictioanry = null);
}
