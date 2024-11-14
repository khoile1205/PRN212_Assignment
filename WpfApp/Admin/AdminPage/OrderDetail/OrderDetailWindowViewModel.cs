using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Models;
using Service.DTO.Admin;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.BaseViewModel;
using WpfApp.Core.Dialog;

namespace WpfApp.Admin.AdminPage.OrderDetail
{
    public partial class OrderDetailWindowViewModel : ObservableObject
    {
        private readonly IOrderService _orderService;
        private readonly IDialogService _dialogService;

        [ObservableProperty]
        private Order _selectedOrder;

        [ObservableProperty]
        private Order _selectedOrderDetail;

        public OrderDetailWindowViewModel(IOrderService orderService, IDialogService dialogService)
        {
            _orderService = orderService;
            _dialogService = dialogService;
        }

        partial void OnSelectedOrderChanged(Order? value)
        {
            if (value != null)
            {
                LoadSelectedOrderDetail();
            }
        }

        private async void LoadSelectedOrderDetail()
        {
            if (SelectedOrder == null)
            {
                return;
            }

            SelectedOrderDetail = await _orderService.GetOrderDetailById(SelectedOrder.Id);
        }

        [RelayCommand]
        private async void CloseDialog()
        {
            _dialogService.CloseDialog<OrderDetailWindow>();
        }
    }
}
