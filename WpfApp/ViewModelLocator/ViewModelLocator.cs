using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Admin;
using WpfApp.Login;
using WpfApp.Register;

namespace WpfApp
{
    public class ViewModelLocator
    {
        public IServiceProvider ServiceProvider { get; }

        public ViewModelLocator()
        {
            ServiceProvider = ((App)Application.Current).ServiceProvider;
        }

        public LoginPageViewModel LoginPageViewModel => ServiceProvider.GetRequiredService<LoginPageViewModel>();
        public MainAdminWindowsViewModel MainAdminWindowsViewModel => ServiceProvider.GetRequiredService<MainAdminWindowsViewModel>();
        public RegisterWindowViewModel RegisterWindowViewModel => ServiceProvider.GetRequiredService<RegisterWindowViewModel>();



    }
}
