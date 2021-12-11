using DevExpress.Mvvm;
using LZ.Compressions.UI.Models;
using LZ.Compressions.UI.ViewModels.CompressorViewModels;
using System.Collections.ObjectModel;

namespace LZ.Compressions.UI.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        public ObservableCollection<CompressorModel> CompressorModels { get; }

        public string MainWindowTitle => "Алгоритмы сжатия без потерь";

        public CompressorViewModel SelectedCompressorViewModel
        {
            get { return GetValue<CompressorViewModel>(nameof(SelectedCompressorViewModel)); }
            set { SetValue(value, nameof(SelectedCompressorViewModel)); }
        }

        public MainViewModel()
        {
            CompressorModels = new ObservableCollection<CompressorModel>
            {
                new CompressorModel(this, new LZWViewModel()),
                new CompressorModel(this, new LZ77ViewModel()),
                new CompressorModel(this, new LZ78ViewModel()),
                new CompressorModel(this, new RLEViewModel())
            };
            CompressorModels[0].IsSelected = true;
        }
    }
}
