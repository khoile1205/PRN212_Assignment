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
using WpfApp.Register;
using WpfApp.Admin.AdminPage.DashboardPage;
using WpfApp.Admin.AdminPage.CashierPage;
using WpfApp.Admin.AdminPage.CustomerPage;
using WpfApp.Admin.AdminPage.ProductPage;
using WpfApp.Core.Dialog;
using WpfApp.Core.Components;
using WpfApp.Utils;

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

            // Configure additional services (likely located in Service.DI)
            DependencyInjection.ConfigureServices(serviceCollection, Configuration);

            serviceCollection.AddSingleton<IUploadService>(provider => new UploadService(Path.Combine(Environment.CurrentDirectory, "Images")));

            // Build the service provider
            ServiceProvider = serviceCollection.BuildServiceProvider();


            // Initialize the first screen (LoginPage in this case)
            OnInitializeScreen();
        }

        private void RegistryPageView(ServiceCollection service)
        {
            service.AddSingleton<IDialogService, DialogService>();
            //service.AddSingleton<ConfirmationDialog>();

            #region Admin

            service.AddSingleton<MainAdminWindows>();
            service.AddSingleton<MainAdminWindowsViewModel>();
            service.AddSingleton<AdminDashboardPage>();
            service.AddSingleton<AdminDashboardViewModel>();
            service.AddSingleton<AdminCashierPage>();
            service.AddSingleton<AdminCashierViewModel>();
            service.AddSingleton<AdminCustomerPage>();
            service.AddSingleton<AdminCustomerViewModel>();
            service.AddSingleton<AdminProductPage>();
            service.AddSingleton<AdminProductViewModel>();

            #endregion

            #region Login
            service.AddSingleton<LoginPage>();
            service.AddSingleton<LoginPageViewModel>();
            #endregion

            #region Register
            service.AddSingleton<RegisterWindow>();
            service.AddSingleton<RegisterWindowViewModel>();
            #endregion
        }

        private void OnInitializeScreen()
        {
            var dialogService = ServiceProvider.GetRequiredService<IDialogService>();
            dialogService.ShowDialog<LoginPage>();
        }
    }

}
