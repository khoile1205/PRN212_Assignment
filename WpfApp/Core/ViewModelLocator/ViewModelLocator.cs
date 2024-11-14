using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Admin;
using WpfApp.Admin.AdminPage.CashierPage;
using WpfApp.Admin.AdminPage.CustomerPage;
using WpfApp.Admin.AdminPage.DashboardPage;
using WpfApp.Admin.AdminPage.OrderDetail;
using WpfApp.Admin.AdminPage.ProductPage;
using WpfApp.Login;
using WpfApp.Register;

namespace WpfApp.Core.ViewModelLocator
{
    public class ViewModelLocator
    {
        public IServiceProvider ServiceProvider { get; }

        public ViewModelLocator()
        {
            ServiceProvider = ((App)Application.Current).ServiceProvider;
        }

        public LoginPageViewModel LoginPageViewModel => ServiceProvider.GetRequiredService<LoginPageViewModel>();
        public RegisterWindowViewModel RegisterWindowViewModel => ServiceProvider.GetRequiredService<RegisterWindowViewModel>();
        public MainAdminWindowsViewModel MainAdminWindowsViewModel => ServiceProvider.GetRequiredService<MainAdminWindowsViewModel>();
        public AdminDashboardViewModel AdminDashboardViewModel => ServiceProvider.GetRequiredService<AdminDashboardViewModel>();
        public AdminCustomerViewModel AdminCustomerViewModel => ServiceProvider.GetRequiredService<AdminCustomerViewModel>();
        public AdminProductViewModel AdminProductViewModel => ServiceProvider.GetRequiredService<AdminProductViewModel>();
        public AdminCashierViewModel AdminCashierViewModel => ServiceProvider.GetRequiredService<AdminCashierViewModel>();
        public OrderDetailWindowViewModel OrderDetailWindowViewModel => ServiceProvider.GetRequiredService<OrderDetailWindowViewModel>();




    }
}
