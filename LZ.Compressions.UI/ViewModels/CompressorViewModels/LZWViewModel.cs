using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Services;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    public class LZWViewModel : CompressorViewModel
    {
        public override string Title => "LZW";
        public override string Decryption => "Lempel Ziv Welch";
        public override bool CanShowReadableView => false;

        public LZWViewModel(LZWCompressor compressor, ITimerService timer) : base(compressor, timer)
        {
        }
    }
}
