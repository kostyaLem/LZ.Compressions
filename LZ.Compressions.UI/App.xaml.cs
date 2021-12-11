using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Services;
using LZ.Compressions.UI.ViewModels;
using LZ.Compressions.UI.ViewModels.CompressorViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LZ.Compressions.UI
{
    public partial class App : Application
    {
        public static ServiceProvider ServiceProvider;

        public App()
        {
            ConfigureServices();
        }

        private void ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddScoped<MainWindow>();
            serviceCollection.AddSingleton<MainViewModel>();

            serviceCollection.AddTransient<ITimerService, TimerService>();


            serviceCollection.AddSingleton<LZWCompressor>();
            serviceCollection.AddSingleton<RLECompressor>();

            serviceCollection.AddSingleton<LZWViewModel>();
            serviceCollection.AddSingleton<LZ77ViewModel>();
            serviceCollection.AddSingleton<LZ78ViewModel>();
            serviceCollection.AddSingleton<RLEViewModel>();

            serviceCollection.AddSingleton<CompressorViewModel, LZWViewModel>();
            serviceCollection.AddSingleton<CompressorViewModel, LZ77ViewModel>();
            serviceCollection.AddSingleton<CompressorViewModel, LZ78ViewModel>();
            serviceCollection.AddSingleton<CompressorViewModel, RLEViewModel>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();
        }
    }
}
