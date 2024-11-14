using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.DTO.Admin
{
    public class StatisticDashboardResponseDto
    {
        public int TotalCashiers { get; set; }
        public int TotalDailyOrders { get; set; }
        public decimal TotalDailyIncome { get; set; }
        public decimal TotalIncome { get; set; }
    }
}
