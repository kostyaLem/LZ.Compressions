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
        public override IReadOnlyList<CompressExample> Examples => throw new System.NotImplementedException();

        public RLEViewModel(RLECompressor compressor, ITimerService timer) : base(compressor, timer)
        {
        }
    }
}
