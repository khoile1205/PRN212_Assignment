using DataAccess.Models;
using DataAccess.UnitOfWork;
using Service.DTO.OrderDto;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WpfApp.Cashier
{
    /// <summary>
    /// Interaction logic for CashierOrder.xaml
    /// </summary>
    public partial class CashierOrder : Window
    {
        private readonly IProductService _productService;
        private readonly ObservableCollection<OrderProductDto> _orderProduct;
        private float price = 0;
        public CashierOrder(IProductService productService)
        {
            new ObservableCollection<OrderProductDto>();
            _productService = productService;
            InitializeComponent();
        }
        private async void Add_Click(object sender, RoutedEventArgs e)
        {
            var addProduct = GetProductOrder();
            if (addProduct is not null)
            {
                var allProducts = await _productService.GetAllProducts();
                var productDetails = allProducts.FirstOrDefault(p => p.Id == addProduct.ProductId);
                if (productDetails is null)
                {
                    MessageBox.Show("Product not found in the product list.", "Add Product", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var productPrice = productDetails.Price;

                var existingProduct = _orderProduct.FirstOrDefault(p => p.ProductId == addProduct.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.Quantity += addProduct.Quantity;
                    price += (float)(productPrice * addProduct.Quantity);
                }
                else
                {
                    _orderProduct.Add(addProduct);
                     price += (float)(productPrice * addProduct.Quantity);
                }

                MessageBox.Show($"Total Price: {price:C}", "Order Summary", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            LoadPriceCart();
            LoadCart();
        }
        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgDataCart.SelectedItem is OrderProductDto selectedProduct)
            {
                var existingProduct = _orderProduct.FirstOrDefault(p => p.ProductId == selectedProduct.ProductId);
                if (existingProduct != null)
                {
                    var allProducts = _productService.GetAllProducts().Result;
                    var productDetails = allProducts.FirstOrDefault(p => p.Id == selectedProduct.ProductId);

                    if (productDetails is not null)
                    {
                        var productPrice = productDetails.Price;
                        price -= (float)(productPrice * existingProduct.Quantity);
                    }

                    _orderProduct.Remove(existingProduct);

                    MessageBox.Show($"Total Price: {price:C}", "Order Summary", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Product not found in the order list.", "Delete Product", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            LoadPriceCart();
            LoadCart();
        }


        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            dgDataCart.ItemsSource = "";
        }

        private void dgData_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataContext = dgData.SelectedItem;
        }
        private async void Load()
        {
            dgData.ItemsSource = await _productService.GetAllProducts();
        }
        private OrderProductDto GetProductOrder()
        {
            var product = new OrderProductDto();
            if(dgData.SelectedItem is Product selected)
            {
                product.ProductId = selected.Id;
                product.CreatedTimestamp =  DateTime.Now;
                product.Quantity = int.Parse(txtQuantity.Text);
            }
            return product;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Receipt_Click(object sender, RoutedEventArgs e)
        {

        }
        private void LoadPriceCart()
        {
            txtPriceCart.Text = price.ToString();
        }
        private void LoadCart()
        {
            dgDataCart.ItemsSource = _orderProduct;
        }
    }
}
