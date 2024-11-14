using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Models;
using Microsoft.Extensions.DependencyInjection;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Admin.AdminPage.OrderDetail;
using WpfApp.Core.BaseViewModel;
using WpfApp.Core.Dialog;

namespace WpfApp.Admin.AdminPage.CustomerPage
{
    public partial class AdminCustomerViewModel : ObservableObject
    {

        private readonly IOrderService _orderService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDialogService _dialogService;

        public ObservableCollection<Order> ListOrders { get; } = new();
        public AdminCustomerViewModel(IOrderService orderService, IServiceProvider serviceProvider, IDialogService dialogService)
        {
            _orderService = orderService;
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;

            LoadOrdersAsync();
        }

        [ObservableProperty]
        private DateTime? _startDate;

        [ObservableProperty]
        private DateTime? _endDate;

        [ObservableProperty]
        private Order _selectedOrder;

        [RelayCommand]
        private async void LoadOrdersAsync()
        {
            ListOrders.Clear();
            // Check if endDate is before startDate
            if (StartDate.HasValue && EndDate.HasValue && EndDate < StartDate)
            {
                MessageBox.Show("End Date cannot be before Start Date", "Invalid Date Range", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var listOrders = await _orderService.GetListOrders(StartDate, EndDate);

            foreach (var order in listOrders)
            {
                ListOrders.Add(order);
            }
        }

        partial void OnSelectedOrderChanged(Order? value)
        {
            if (value != null)
            {
                // Ensure that the OrderDetailWindowViewModel gets the correct SelectedOrder
                var orderDetailWindowViewModel = _serviceProvider.GetRequiredService<OrderDetailWindowViewModel>();
                orderDetailWindowViewModel.SelectedOrder = value;  // Set the SelectedOrder first

                // Show the OrderDetailWindow after ensuring the order is set
                _dialogService.ShowDialog<OrderDetailWindow>();
            }
        }
    }
}
