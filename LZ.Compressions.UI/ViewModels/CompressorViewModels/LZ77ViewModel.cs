using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Services;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    public class LZ77ViewModel : CompressorViewModel
    {
        private readonly IReadableCompressor _compressor;

        public override string Title => "LZ77";
        public override string Decryption => "Lempel Ziv 77";
        public override bool CanShowReadableView => true;

        public LZ77ViewModel(LZ77Compressor compressor, ITimerService timer) : base(compressor, timer)
        {
            _compressor = compressor;
            DataCompressed += OnDataCompressed;
        }

        private void OnDataCompressed(object? sender, System.EventArgs e)
        {
            ReadableView = _compressor.GetReadableView(OutputString);
        }
    }
}
