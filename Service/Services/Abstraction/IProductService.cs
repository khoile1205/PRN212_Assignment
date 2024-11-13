using DataAccess.Models;
using Service.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services.Abstraction
{
    public interface IProductService
    {
        public Task<IEnumerable<Product>> GetAllProducts();
        public Task<Product> AddProduct(Product product);
        public Task<Product> UpdateProduct(Product product);
        public Task<bool> DeleteProduct(int productId);
        public Task<Product?> GetProductById(int productId);
    }
}
