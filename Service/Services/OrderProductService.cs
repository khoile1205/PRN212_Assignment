using DataAccess.Models;
using DataAccess.UnitOfWork;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class OrderProductService : IOrderProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderProductService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task AddOrderProduct(OrderProduct product)
        {
            await _unitOfWork.OrderProducts.Add(product);
        }
    }
}
