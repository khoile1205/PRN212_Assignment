using CommunityToolkit.Mvvm.ComponentModel;
using Service.DTO.Admin;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WpfApp.Core.BaseViewModel;

namespace WpfApp.Admin.AdminPage.DashboardPage
{
    public partial class AdminDashboardViewModel : ObservableObject
    {
        private readonly IDashboardService _dashboardService;

        public AdminDashboardViewModel(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;

            LoadStatisticDashboardAsync();
        }

        // Observable property for the dashboard statistics data
        [ObservableProperty]
        private StatisticDashboardResponseDto statisticDashboard;

        // Async method to load the statistics data
        private async Task LoadStatisticDashboardAsync()
        {
            try
            {
                var data = await _dashboardService.GetStatisticDashboardResponse();
                StatisticDashboard = data; // Set the data to the property
            }
            catch (Exception ex)
            {
                // Handle any exceptions (e.g., show error message or log)
                // For simplicity, we won't display it here, but you could show a message in the UI
                Console.WriteLine(ex.Message);
            }
        }
    }
}
