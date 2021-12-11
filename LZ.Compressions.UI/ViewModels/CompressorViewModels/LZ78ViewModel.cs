using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Services;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    public class LZ78ViewModel : CompressorViewModel
    {
        public override string Title => "LZ78";
        public override string Decryption => "Lempel Ziv 78";

        public LZ78ViewModel(LZWCompressor compressor, ITimerService timer) : base(compressor, timer)
        {
        }
    }
}
