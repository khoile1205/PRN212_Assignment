using DataAccess.Models;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Service.Enums;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }


        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            try
            {
                return await _unitOfWork.Products.GetAll(null, p => p.Include(p => p.ProductStocks).Include(p => p.OrderProducts));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving products: {ex.Message}");
                throw new Exception("Could not retrieve users.", ex);
            }
        }

        public async Task<Product> AddProduct(Product product)
        {
            try
            {
                return await _unitOfWork.Products.Add(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while adding the product: {ex.Message}");
                throw new Exception("Could not add the product.", ex);
            }
        }

        public async Task<Product> UpdateProduct(Product product)
        {
            try
            {
                return await _unitOfWork.Products.Update(product);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating the product: {ex.Message}");
                throw new Exception("Could not update the product.", ex);
            }
        }

        public async Task<bool> DeleteProduct(int productId)
        {
            try
            {
                Product product = await _unitOfWork.Products.GetFirstOrDefault(p => p.Id == productId);

                if (product == null)
                {
                    throw new Exception("Product not found.");
                }

                await _unitOfWork.Products.Delete(product);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while deleting the user: {ex.Message}");
                throw new Exception("Could not delete the user.", ex);
            }
        }

        public async Task<Product?> GetProductById(int productId)
        {
            try
            {
                return await _unitOfWork.Products.GetFirstOrDefault(p => p.Id == productId, q => q.Include(u => u.ProductStocks).Include(p => p.OrderProducts));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while retrieving the user: {ex.Message}");
                throw new Exception("Could not retrieve the user.", ex);
            }
        }
    }
}
