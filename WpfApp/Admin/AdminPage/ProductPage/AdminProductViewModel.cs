using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Models;
using Microsoft.IdentityModel.Tokens;
using Service.Services;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfApp.Core.BaseViewModel;
using WpfApp.Utils;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace WpfApp.Admin.AdminPage.ProductPage
{
    public partial class AdminProductViewModel : ObservableObject
    {
        private readonly IProductService _productService;
        private readonly IImageService _imageService;

        public ObservableCollection<string> TypeOptions { get; } = new() { "Meal", "Drink" };
        public ObservableCollection<Product> ListProducts { get; } = new();
        public ObservableCollection<string> StatusOptions { get; } = new() { "Available", "Unavailable" };

        public AdminProductViewModel(IProductService productService, IImageService imageService)
        {
            _productService = productService;
            _imageService = imageService;

            InitializeViewModelAsync();
        }


        [ObservableProperty]
        private BitmapImage? _productImage;

        [ObservableProperty]
        private string? _productImagePath;

        [ObservableProperty]
        private string? _selectedType;

        [ObservableProperty]
        private string? _selectedStatus;

        [ObservableProperty]
        private string? _productName;

        [ObservableProperty]
        private int? _productStock;

        [ObservableProperty]
        private string? _productType;

        [ObservableProperty]
        private decimal? _productPrice;

        [ObservableProperty]
        private Product? _selectedProduct;

        private async Task InitializeViewModelAsync()
        {
            await LoadProductsAsync();
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                ListProducts.Clear();
                var listProducts = await _productService.GetAllProducts();
                foreach (var product in listProducts)
                {
                    ListProducts.Add(product);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load cashiers: {ex.Message}");
            }
        }

        partial void OnSelectedProductChanged(Product? value)
        {
            if (value != null)
            {
                ProductName = value.ProductName;
                ProductImagePath = value.Image;
                SelectedType = value.Type;
                SelectedStatus = value.Status;
                ProductStock = value.Stock;
                ProductPrice = value.Price;

                //Load the image if needed
                if (!string.IsNullOrEmpty(value.Image))
                {
                    ProductImage = _imageService.LoadImage(value.Image);
                }
                else
                {
                    ProductImage = null;
                }
            }
            else
            {
                ClearFields();
            }
        }

        [RelayCommand]
        private void ImportImage()
        {
            try
            {
                var (image, path) = _imageService.UploadImage();
                if (image != null && path != null)
                {
                    ProductImage = image;
                    ProductImagePath = path;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to import image: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task AddProductAsync()
        {
            if (!ValidateProductInputs()) return;

            try
            {
                var newProduct = new Product
                {
                    ProductName = ProductName,
                    Image = ProductImagePath,
                    CreatedTimestamp = DateTime.Now,
                    Stock = ProductStock,
                    Type = SelectedType,
                    Status = SelectedStatus,
                    Price = ProductPrice
                };

                await _productService.AddProduct(newProduct);
                _imageService.SaveImage(ProductImage, ProductImagePath);
                await LoadProductsAsync();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add cashier: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ClearFields()
        {
            if (SelectedProduct != null)
            {
                SelectedProduct = null;
            }

            ProductName = string.Empty;
            ProductStock = null;
            SelectedType = null;
            SelectedStatus = null;
            ProductPrice = null;
            ProductImage = null;
            ProductImagePath = string.Empty;

        }

        [RelayCommand]
        private async Task UpdateProductAsync()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select a product to update.");
                return;
            }

            if (!ValidateProductInputs()) return;

            try
            {
                var updatedProduct = new Product
                {
                    Id = SelectedProduct.Id,
                    ProductName = ProductName,
                    Image = ProductImagePath,
                    CreatedTimestamp = SelectedProduct.CreatedTimestamp,
                    UpdatedTimestamp = DateTime.Now,
                    Stock = ProductStock,
                    Type = SelectedType,
                    Status = SelectedStatus,
                    Price = ProductPrice
                };

                await _productService.UpdateProduct(updatedProduct);
                _imageService.SaveImage(ProductImage, ProductImagePath);
                await LoadProductsAsync();
                MessageBox.Show("Product updated successfully.");
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update product: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task DeleteProductAsync()
        {
            if (SelectedProduct == null)
            {
                MessageBox.Show("Please select a product to delete.");
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {SelectedProduct.ProductName}?",
                                         "Confirm Delete", MessageBoxButton.YesNo);
            if (result != MessageBoxResult.Yes) return;

            try
            {
                await _productService.DeleteProduct(SelectedProduct.Id); // Assuming ProductId is the key identifier
                ClearFields();
                await LoadProductsAsync();
                MessageBox.Show("Product deleted successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete product: {ex.Message}");
            }
        }

        private bool ValidateProductInputs()
        {
            if (string.IsNullOrEmpty(ProductName))
            {
                MessageBox.Show("Username is required.");
                return false;
            }

            if (string.IsNullOrEmpty(ProductStock.ToString()) || !int.TryParse(ProductStock.ToString(), out int stockValue))
            {
                MessageBox.Show("Please enter a valid number for the stock.");
                return false;
            }

            // Optionally, check if the number is negative
            if (ProductStock < 0)
            {
                MessageBox.Show("Stock cannot be a negative number.");
                return false;
            }

            if (string.IsNullOrEmpty(SelectedType))
            {
                MessageBox.Show("Type is required.");
                return false;
            }

            if (string.IsNullOrEmpty(SelectedStatus))
            {
                MessageBox.Show("Status is required.");
                return false;
            }

            if (string.IsNullOrEmpty(ProductPrice.ToString()) || !decimal.TryParse(ProductPrice.ToString(), out decimal price))
            {
                MessageBox.Show("Please enter a valid number for the price.");
                return false;
            }

            // Optionally, check if the number is negative
            if (ProductStock < 0)
            {
                MessageBox.Show("Price cannot be a negative number.");
                return false;
            }

            return true;
        }
    }
}
