using CommunityToolkit.Mvvm.ComponentModel;
using DataAccess.Models;
using Service.DTO.Admin;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp.Core.Dialog;

namespace WpfApp.Cashier
{
    /// <summary>
    /// Interaction logic for DashboardCashier.xaml
    /// </summary>
    public partial class DashboardCashier : Window
    {
        private readonly IDashboardService _dashboardService;
        private readonly IDialogService _dialogService;
        public ObservableCollection<Order> ListDailyOrders { get; set; } = new ObservableCollection<Order>();

        public DashboardCashier(IDashboardService dashboardService, IDialogService dialogService)
        {
            _dashboardService = dashboardService;
            _dialogService = dialogService;
            InitializeComponent();
            LoadStatisticDashboardAsync();
        
        }
        public async Task LoadStatisticDashboardAsync()
        {
            try
            {
                var data = await _dashboardService.GetStatisticDashboardResponse();
                DataContext = data;
                ListDailyOrders = new ObservableCollection<Order>(data.ListDailyOrders);
                Load();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        private void Load()
        {
            dgvOrderDailyData.ItemsSource = ListDailyOrders;
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            _dialogService.CloseDialog<DashboardCashier>();
        }
    }
}
