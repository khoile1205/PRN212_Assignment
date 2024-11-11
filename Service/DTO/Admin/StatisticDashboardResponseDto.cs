using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO.Admin
{
    public class StatisticDashboardResponseDto
    {
        private int TotalCashiers { get; set; }
        private int TotalCustomers { get; set; }
        private int TotalDailyIncome { get; set; }
        private int TotalDailyOrders { get; set; }
    }
}
