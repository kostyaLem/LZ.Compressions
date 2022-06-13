using DevExpress.Mvvm;
using LZ.Compressions.UI.Models;
using LZ.Compressions.UI.ViewModels.CompressorViewModels;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace LZ.Compressions.UI.ViewModels
{
    internal class MainViewModel : ViewModelBase
    {
        // Список алгоритмов сжатя для отображения на интерфейсе
        public ObservableCollection<CompressorModel> CompressorModels { get; }

        // Заголовок окна
        public string MainWindowTitle => "Алгоритмы сжатия без потерь";

        // Выбранная модель сжатия
        public CompressorViewModel SelectedCompressorViewModel
        {
            get { return GetValue<CompressorViewModel>(nameof(SelectedCompressorViewModel)); }
            set { SetValue(value, nameof(SelectedCompressorViewModel)); }
        }

        public MainViewModel(IEnumerable<CompressorViewModel> compressorViewModels)
        {
            CompressorModels = new ObservableCollection<CompressorModel>
            (
                compressorViewModels.Select(vm => new CompressorModel(this, vm))
            );
            CompressorModels[0].IsSelected = true;
        }
    }
}
