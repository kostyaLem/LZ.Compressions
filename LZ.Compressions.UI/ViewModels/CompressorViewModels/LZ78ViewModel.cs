using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Models;
using LZ.Compressions.UI.Services;
using System.Collections.Generic;

namespace LZ.Compressions.UI.ViewModels.CompressorViewModels
{
    internal class LZ78ViewModel : CompressorViewModel
    {
        public override string Title => "LZ78";
        public override string Caption => "Lempel Ziv 78";
        public override IReadOnlyList<CompressExample> Examples => throw new System.NotImplementedException();

        public LZ78ViewModel(LZ78Compressor compressor, ITimerService timer) : base(compressor, timer)
        {
        }
    }
}
