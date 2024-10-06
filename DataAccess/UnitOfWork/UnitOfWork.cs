using DataAccess.Models;
using DataAccess.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Prn211AssignmentContext dbContext;
        private Repository<User> _userRepository;
        private Repository<Role> _roleRepository;
        private Repository<UserOrder> _userOrderRepository;
        private Repository<Order> _orderRepository;
        private Repository<Product> _productRepository;


        public UnitOfWork(Prn211AssignmentContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public IRepository<User> Users => _userRepository ??= new Repository<User>(dbContext);
        public IRepository<Role> Roles => _roleRepository ??= new Repository<Role>(dbContext);
        public IRepository<UserOrder> UserOrders => _userOrderRepository ??= new Repository<UserOrder>(dbContext);
        public IRepository<Order> Orders => _orderRepository ??= new Repository<Order>(dbContext);
        public IRepository<Product> Products => _productRepository ??= new Repository<Product>(dbContext);


        public void Dispose()
        {
            dbContext.Dispose();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }
    }
}
