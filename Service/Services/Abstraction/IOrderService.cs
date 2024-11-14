using DataAccess.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Abstraction
{
    public interface IOrderService
    {
        public Task<IEnumerable<Order>> GetListOrders(DateTime? startTime, DateTime? endDate);
        public Task<Order> GetOrderDetailById(int orderId);
    }
}
