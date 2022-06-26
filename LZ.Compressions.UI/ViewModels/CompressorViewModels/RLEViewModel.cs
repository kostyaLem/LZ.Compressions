using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Models;
using LZ.Compressions.UI.Services;
using System.Collections.Generic;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    internal sealed class RLEViewModel : CompressorViewModel
    {
        public override string Title => "RLE";
        public override string Caption => "Run Lenth Encode";
        public override IReadOnlyList<CompressExample> Examples { get; }

        public RLEViewModel(RLECompressor compressor, ITimerService timer) : base(compressor, timer)
        {
            Examples = new List<CompressExample>()
            {
                new CompressExample("AAAAAAAABCCCC", "8A1B4C"),
                new CompressExample("aaabbbccc", "3a3b3c"),
                new CompressExample("aaaaaaaaaa", "10a"),
            };
        }
    }
}
