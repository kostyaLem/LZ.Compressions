using DevExpress.Mvvm;
using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.Core.Exceptions;
using LZ.Compressions.UI.Services;
using System;
using System.Windows;

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
            // Запустить таймер и сжатие строки
            _timer.Start();

            try
            {
                // Вызывать проверку входной строки перед сжатием
                _compressor.ValidateBeforeCompress(DecompressedString);

                var compressed = _compressor.Compress(DecompressedString);

                // Вывести потраченное время и результат
                (CompressedString, CompressedLength) = compressed;
            }
            catch (InputStringValidateException e)
            {
                (CompressedString, CompressedLength) = (string.Empty, 0);
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ElapsedTime = _timer.Stop();
            }
        }

        public virtual void DecompressData()
        {
            // Запустить таймер и сжатие строки
            _timer.Start();

            try
            {
                // Вызывать проверку входной строки перед сжатием
                _compressor.ValidateBeforeDecompress(CompressedString);

                var decompressed = _compressor.Decompress(CompressedString);

                // Вывести потраченное время и результат
                DecompressedString = decompressed;
            }
            catch (InputStringValidateException e)
            {
                DecompressedString = string.Empty;
                MessageBox.Show(e.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                ElapsedTime = _timer.Stop();
            }
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
