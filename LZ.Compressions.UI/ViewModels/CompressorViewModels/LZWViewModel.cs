using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Models;
using LZ.Compressions.UI.Services;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    internal class LZWViewModel : CompressorViewModel
    {
        public override string Title => "LZW";
        public override string Caption => "Lempel Ziv Welch";
        public override IReadOnlyList<CompressExample> Examples { get; }
        public string InitialDictionary
        {
            get { return GetValue<string>(nameof(InitialDictionary)); }
            set { SetValue(value, nameof(InitialDictionary)); }
        }

        public LZWViewModel(LZWCompressor compressor, ITimerService timer) : base(compressor, timer)
        {
            InitialDictionary = string.Empty;
        }

        public override void Compress()
        {
            var compressor = _compressor as LZWCompressor;
            var compressed = compressor!.Compress(DecompressedString);

            (CompressedString, CompressedLength, _) = compressed;
            InitialDictionary = string.Join(Environment.NewLine, compressed.Dictioanry);
        }

        public override void Decompress()
        {
            var compressor = _compressor as LZWCompressor;
            var dictionary = InitialDictionary
                .Split(Environment.NewLine)
                .Select((x, i) => new { Index = i, Str = x })
                .ToDictionary(p => p.Index, x => x.Str);

            DecompressedString = compressor!.Decompress(CompressedString, dictionary);
        }
    }
}
