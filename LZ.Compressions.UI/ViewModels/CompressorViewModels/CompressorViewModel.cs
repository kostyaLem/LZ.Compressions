using DevExpress.Mvvm;
using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Services;
using System;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    public abstract class CompressorViewModel : ViewModelBase
    {
        // Алгоритм сжатия текста
        private readonly ITextCompressor _compressor;
        // Таймер для оценки времени выполнения
        private readonly ITimerService _timer;

        public abstract string Title { get; }
        public abstract string Decryption { get; }
        
        // Команда сжатия
        public DelegateCommand CompressCommand { get; }
        // Команда распаковки
        public DelegateCommand DecompressCommand { get; }
        // Команда сброса текста и статистики
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
            // Вызывать проверку входной строки перед сжатием
            _compressor.ValidateBeforeCompress(DecompressedString);

            // Запустить таймер и сжатие строки
            _timer.Start();
            var compressed = _compressor.Compress(DecompressedString);

            // Вывести потраченное время и результат
            ElapsedTime = _timer.Stop();
            (CompressedString, CompressedLength) = compressed;
        }

        public virtual void DecompressData()
        {
            // Вызывать проверку входной строки перед сжатием
            _compressor.ValidateBeforeDecompress(CompressedString);

            // Запустить таймер и распаковку строки
            _timer.Start();
            var decompressed = _compressor.Decompress(CompressedString);

            // Вывести потраченное время и результат
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
