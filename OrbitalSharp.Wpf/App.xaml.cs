using Microsoft.Extensions.DependencyInjection;
using OrbitalSharp.Wpf.Services;
using OrbitalSharp.Wpf.ViewModel;
using System.Configuration;
using System.Data;
using System.Windows;

namespace OrbitalSharp.Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private IServiceProvider? _serviceProvider;

        protected override void OnStartup(StartupEventArgs e)
        {
            var services = new ServiceCollection();
            ConfigureServices(services);

            _serviceProvider = services.BuildServiceProvider();

            var mainWindow = new MainWindow();
            var dialogService = new DialogService(mainWindow);

            var mainViewModel = new MainViewModel(dialogService);
            mainWindow.DataContext = mainViewModel;
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IDialogService, DialogService>();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            if (_serviceProvider is IDisposable disposable)
            {
                disposable.Dispose();
            }
        }
    }

}
