using DataAccess.Models;
using DataAccess.UnitOfWork;
using Service.DTO.Admin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.Enums.AppEnums;

namespace Service.Services.Abstraction
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUserService _userService;
        public DashboardService(IUnitOfWork unitOfWork, IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }
        public async Task<StatisticDashboardResponseDto> GetStatisticDashboardResponse()
        {

            IEnumerable<User> listCashier = await _userService.GetListUserByRole(ROLE_ENUMS.Cashier);
            IEnumerable<Order> listOrders = await _unitOfWork.Orders.GetAll();
            IEnumerable<Order> listDailyOrders = listOrders.Where(o => o.CreatedTimestamp.HasValue && o.CreatedTimestamp.Value.Date == DateTime.Today);

            StatisticDashboardResponseDto response = new StatisticDashboardResponseDto
            {
                TotalCashiers = listCashier.Count(),
                TotalDailyIncome = listDailyOrders.Sum(o => o.TotalOrderPrice),
                TotalDailyOrders = listDailyOrders.Count(),
                TotalIncome = listOrders.Sum(o => o.TotalOrderPrice)
            };

            return response;
        }
    }
}
