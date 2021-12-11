using LZ.Compressions.Core.Algorithms;
using LZ.Compressions.UI.Services;
using LZ.Compressions.UI.ViewModels.CompressorViewModels;
using Microsoft.Extensions.DependencyInjection;
using System.Windows;

namespace LZ.Compressions.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
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

            serviceCollection.AddTransient<ITimerService, TimerService>();

            serviceCollection.AddSingleton<ITextCompressor, LZWCompressor>();

            serviceCollection.AddSingleton<CompressorViewModel, LZWViewModel>();

            ServiceProvider = serviceCollection.BuildServiceProvider();
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = ServiceProvider.GetService<MainWindow>();
            mainWindow.Show();
        }
    }
}
