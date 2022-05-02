using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Services;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    public sealed class RLEViewModel : CompressorViewModel
    {
        public override string Title => "RLE";
        public override string Decryption => "Run Lenth Encode";

        public RLEViewModel(RLECompressor compressor, ITimerService timer) : base(compressor, timer)
        {
        }
    }
}
