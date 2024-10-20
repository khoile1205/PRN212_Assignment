using Core.Command;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WpfApp.Admin.AdminPage.CashierPage;
using WpfApp.Admin.AdminPage.CustomerPage;
using WpfApp.Admin.AdminPage.DashboardPage;
using WpfApp.Admin.AdminPage.ProductPage;
using WpfApp.Core.BaseViewModel;
using WpfApp.Core.Dialog;

namespace WpfApp.Admin
{
    public class MainAdminWindowsViewModel : BaseViewModel
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDialogService dialogService;

        #region ICommand
        public ICommand LogoutCommand { get; }
        public ICommand NavigatePage { get; }
        #endregion

        public MainAdminWindowsViewModel(IServiceProvider serviceProvider, IDialogService dialogService)
        {
            this.serviceProvider = serviceProvider;
            this.dialogService = dialogService;
            LogoutCommand = new RelayCommand(LogoutExecute);
            NavigatePage = new RelayCommand(NavigatePageExecute);
        }

        private void LogoutExecute(object obj)
        {
            dialogService.CloseDialog<MainAdminWindows>();
            dialogService.ShowDialog<LoginPage>();
        }

        public void NavigatePageExecute(object parameter)
        {
            if (parameter is string page)
            {
                Page newPage = null;

                switch (page)
                {
                    case WPFAdminConstants.ADMIN_DASHBOARD_PAGE:
                        {
                            newPage = serviceProvider.GetRequiredService<AdminDashboardPage>();
                            break;
                        }
                    case WPFAdminConstants.ADMIN_CUSTOMER_PAGE:
                        {
                            newPage = serviceProvider.GetRequiredService<AdminCustomerPage>();
                            break;
                        }
                    case WPFAdminConstants.ADMIN_PRODUCTS_PAGE:
                        {
                            newPage = serviceProvider.GetRequiredService<AdminProductPage>();
                            break;
                        }
                    case WPFAdminConstants.ADMIN_CASHIER_PAGE:
                        {
                            newPage = serviceProvider.GetRequiredService<AdminCashierPage>();
                            break;
                        }
                }

                if (newPage != null)
                {
                    (Application.Current.MainWindow as MainAdminWindows)?.MainFrame.Navigate(newPage);
                }
            }
        }
    }
}
