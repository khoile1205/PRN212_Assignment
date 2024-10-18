using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Service.DI;
using Service.Services.Abstraction;
using Service.Services;
using System.Configuration;
using System.Data;
using System.IO;
using System.Windows;
using WpfApp.Admin;
using WpfApp.Login;
using WpfApp.Dialog;
using WpfApp.Register;

namespace WpfApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }
        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set up configuration
            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            // Register ViewModels and Views
            RegistryPageView(serviceCollection);
            RegistryServiceViewModel(serviceCollection);
            // Configure additional services (likely located in Service.DI)
            DependencyInjection.ConfigureServices(serviceCollection, Configuration);

            // Build the service provider
            ServiceProvider = serviceCollection.BuildServiceProvider();

            // Initialize the first screen (LoginPage in this case)
            OnInitializeScreen();
        }

        private void RegistryPageView(ServiceCollection service)
        {
            service.AddSingleton<IDialogService, DialogService>();

            service.AddTransient<LoginPage>();
            service.AddTransient<MainAdminWindows>();
            service.AddTransient<RegisterWindow>();
        }
        private void RegistryServiceViewModel(ServiceCollection service)
        {
            service.AddTransient<LoginPageViewModel>();
            service.AddTransient<MainAdminWindowsViewModel>();
            service.AddTransient<RegisterWindowViewModel>();
        }

        private void OnInitializeScreen()
        {
            var dialogService = ServiceProvider.GetRequiredService<IDialogService>();
            dialogService.ShowDialog<LoginPage>();
        }
    }

}
