using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Services;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    public class RLEViewModel : CompressorViewModel
    {
        private readonly IReadableCompressor _readableCompressor;

        public override string Title => "RLE";
        public override string Decryption => "Run Lenth Encode";

        public override bool CanShowReadableView => true;

        public RLEViewModel(RLECompressor compressor, ITimerService timer) : base(compressor, timer)
        {
            _readableCompressor = compressor;
            DataCompressed += OnDataCompressed;
        }

        private void OnDataCompressed(object? sender, System.EventArgs e)
        {
            ReadableView = _readableCompressor.GetReadableView(OutputString);
        }
    }
}
