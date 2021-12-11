using DevExpress.Mvvm;
using LZ.Compressions.UI.ViewModels;
using LZ.Compressions.UI.ViewModels.CompressorViewModels;

namespace LZ.Compressions.UI.Models
{
    internal class CompressorModel : BindableBase
    {
        private readonly MainViewModel _mainViewModel;

        public CompressorViewModel CompressorViewModel
        {
            get { return GetValue<CompressorViewModel>(nameof(CompressorViewModel)); }
            set { SetValue(value, nameof(CompressorViewModel)); }
        }

        public bool IsSelected
        {
            get { return GetValue<bool>(nameof(IsSelected)); }
            set
            {
                if (value)
                {
                    _mainViewModel.SelectedCompressorViewModel = CompressorViewModel;
                }
                SetValue(value, nameof(IsSelected));
            }
        }

        public CompressorModel(MainViewModel mainViewModel, CompressorViewModel compressorViewModel)
        {
            _mainViewModel = mainViewModel;
            CompressorViewModel = compressorViewModel;
        }
    }
}
