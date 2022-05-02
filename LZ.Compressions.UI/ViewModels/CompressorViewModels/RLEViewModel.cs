using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Services;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    public sealed class RLEViewModel : CompressorViewModel
    {
        private readonly ITextCompressor _compressor;

        public override string Title => "RLE";
        public override string Decryption => "Run Lenth Encode";

        public override bool CanShowReadableView => true;

        public RLEViewModel(RLECompressor compressor, ITimerService timer) : base(compressor, timer)
        {
            _compressor = compressor;
        }

        private void OnDataCompressed(object? sender, System.EventArgs e)
        {
            ReadableView = _compressor.Compress(OutputString);
        }
    }
}
