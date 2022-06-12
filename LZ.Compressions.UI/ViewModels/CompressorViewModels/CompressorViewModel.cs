using DevExpress.Mvvm;
using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Services;
using System;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    public abstract class CompressorViewModel : ViewModelBase
    {
        private readonly ITextCompressor _compressor;
        private readonly ITimerService _timer;

        public abstract string Title { get; }
        public abstract string Decryption { get; }

        public DelegateCommand CompressCommand { get; }
        public DelegateCommand DecompressCommand { get; }
        public DelegateCommand ClearCommand { get; }

        public CompressorViewModel(ITextCompressor compressor, ITimerService timer)
        {
            _compressor = compressor;
            _timer = timer;

            CompressCommand = new DelegateCommand(CompressData, () => !string.IsNullOrWhiteSpace(DecompressedString));
            DecompressCommand = new DelegateCommand(DecompressData, () => !string.IsNullOrWhiteSpace(CompressedString));
            ClearCommand = new DelegateCommand(ClearData);
        }

        public string DecompressedString
        {
            get { return GetValue<string>(nameof(DecompressedString)); }
            set { SetValue(value, nameof(DecompressedString)); }
        }

        public string CompressedString
        {
            get { return GetValue<string>(nameof(CompressedString)); }
            set { SetValue(value, nameof(CompressedString)); }
        }

        public int CompressedLength
        {
            get { return GetValue<int>(nameof(CompressedLength)); }
            set { SetValue(value, nameof(CompressedLength)); }
        }

        public TimeSpan ElapsedTime
        {
            get { return GetValue<TimeSpan>(nameof(ElapsedTime)); }
            set { SetValue(value, nameof(ElapsedTime)); }
        }

        public virtual void CompressData()
        {
            _compressor.ValidateBeforeCompress(DecompressedString);

            _timer.Start();
            var compressed = _compressor.Compress(DecompressedString);

            ElapsedTime = _timer.Stop();
            (CompressedString, CompressedLength) = compressed;
        }

        public virtual void DecompressData()
        {
            _compressor.ValidateBeforeDecompress(CompressedString);

            _timer.Start();
            var decompressed = _compressor.Decompress(CompressedString);

            ElapsedTime = _timer.Stop();
            DecompressedString = decompressed;
        }

        public virtual void ClearData()
        {
            ElapsedTime = TimeSpan.Zero;
            DecompressedString = string.Empty;
            CompressedString = string.Empty;
            CompressedLength = 0;
        }
    }
}
