using DataAccess.Models;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Service.Enums;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Service.Enums.AppEnums;

namespace Service.Services
{
    public class OrderService : IOrderService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Order>> GetListOrders(DateTime? startTime, DateTime? endDate)
        {
            try
            {
                var listOrders = await _unitOfWork.Orders.GetAll();

                // If startDate is provided, filter from that date onwards
                if (startTime.HasValue)
                {
                    listOrders = listOrders.Where(order => order.CreatedTimestamp >= startTime.Value);
                }

                // If endDate is provided, filter up to that date
                if (endDate.HasValue)
                {
                    listOrders = listOrders.Where(order => order.CreatedTimestamp <= endDate.Value);
                }

                // If neither startDate nor endDate is provided, fetch all orders without filtering by date
                if (!startTime.HasValue && !endDate.HasValue)
                {
                    // No date filter, return all orders
                    listOrders = listOrders.Where(order => true);
                }

                return listOrders;
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework like Serilog, NLog, etc.)
                Debug.WriteLine($"Error occurred in GetListOrders: {ex.Message}");

                // You can throw a custom exception or rethrow the original exception
                throw new ApplicationException("An error occurred while fetching the orders.", ex);
            }
        }

        public async Task<Order> GetOrderDetailById(int orderId)
        {
            try
            {
                var order = await _unitOfWork.Orders.GetFirstOrDefault(o => o.Id == orderId, o => o.Include(o => o.OrderProducts).ThenInclude(op => op.Product));

                if (order == null)
                {
                    throw new KeyNotFoundException($"Order with ID {orderId} not found.");
                }

                return order;
            }
            catch (Exception ex)
            {
                // Log the exception (e.g., using a logging framework like Serilog, NLog, etc.)
                Debug.WriteLine($"Error occurred in GetOrderDetailById: {ex.Message}");

                // Throw a custom exception or rethrow
                throw new ApplicationException($"An error occurred while fetching the order with ID {orderId}.", ex);
            }
        }
    }
}
