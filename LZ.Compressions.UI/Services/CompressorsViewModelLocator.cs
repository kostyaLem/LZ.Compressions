using LZ.Compressions.UI.ViewModels.CompressorViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace LZ.Compressions.UI.Services
{
    public static class CompressorsViewModelLocator
    {
        public static LZWViewModel LZWViewModel 
            => App.ServiceProvider.GetRequiredService<LZWViewModel>();
    }
}
