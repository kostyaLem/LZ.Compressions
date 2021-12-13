﻿using DevExpress.Mvvm;
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

        public event EventHandler DataCompressed;

        public CompressorViewModel(ITextCompressor compressor, ITimerService timer)
        {
            _compressor = compressor;
            _timer = timer;

            CompressCommand = new DelegateCommand(CompressData, !string.IsNullOrWhiteSpace(InputString));
            DecompressCommand = new DelegateCommand(DecompressData, !string.IsNullOrWhiteSpace(OutputString));
            ClearCommand = new DelegateCommand(ClearData);
        }

        public string InputString
        {
            get { return GetValue<string>(nameof(InputString)); }
            set { SetValue(value, nameof(InputString)); }
        }

        public string OutputString
        {
            get { return GetValue<string>(nameof(OutputString)); }
            set { SetValue(value, nameof(OutputString)); }
        }

        public string ReadableView
        {
            get { return GetValue<string>(nameof(ReadableView)); }
            set { SetValue(value, nameof(ReadableView)); }
        }

        public TimeSpan ElapsedTime
        {
            get { return GetValue<TimeSpan>(nameof(ElapsedTime)); }
            set { SetValue(value, nameof(ElapsedTime)); }
        }

        public abstract bool CanShowReadableView { get; }

        public virtual void CompressData()
        {
            _timer.Start();
            var compressed = _compressor.Compress(InputString);

            ElapsedTime = _timer.Stop();
            OutputString = compressed;

            DataCompressed?.Invoke(this, EventArgs.Empty);
        }

        public virtual void DecompressData()
        {
            _timer.Start();
            var decompressed = _compressor.Decompress(OutputString);

            ElapsedTime = _timer.Stop();
            InputString = decompressed;
        }

        public virtual void ClearData()
        {
            ElapsedTime = TimeSpan.Zero;
            InputString = string.Empty;
            OutputString = string.Empty;
        }
    }
}
