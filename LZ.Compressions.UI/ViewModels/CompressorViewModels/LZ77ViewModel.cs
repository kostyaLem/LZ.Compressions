using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Models;
using LZ.Compressions.UI.Services;
using System.Collections.Generic;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    internal class LZ77ViewModel : CompressorViewModel
    {
        public override string Title => "LZ77";
        public override string Caption => "Lempel Ziv 77";
        public override IReadOnlyList<CompressExample> Examples { get; }

        public LZ77ViewModel(LZ77Compressor compressor, ITimerService timer) : base(compressor, timer)
        {
            Examples = new List<CompressExample>
            {
                new CompressExample("ababcbababaa","(0,0,a), (0,0,b), (2,2,c), (4,3,a), (2,2,a)"),
                new CompressExample("",""),
                new CompressExample("","")
            };
        }
    }
}
