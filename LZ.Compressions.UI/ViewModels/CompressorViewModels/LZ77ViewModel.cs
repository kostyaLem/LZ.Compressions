using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Services;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    public class LZ77ViewModel : CompressorViewModel
    {
        public override string Title => "LZ77";
        public override string Decryption => "Lempel Ziv 77";
        public override bool CanShowReadableView => false;

        public LZ77ViewModel(LZWCompressor compressor, ITimerService timer) : base(compressor, timer)
        {
        }
    }
}
