using DataAccess.Models;
using Service.DTO.OrderDto;
using Service.Services.Abstraction;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Diagnostics;
using WpfApp.Core.Dialog;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;


namespace WpfApp.Cashier
{
    public partial class CashierOrder : Window
    {
        private readonly IDialogService _dialogService;
        private readonly IServiceProvider serviceProvider;
        private readonly IProductService _productService;
        private readonly IOrderService _orderService;
        private readonly IAuthService _authService;
        private readonly IOrderProductService _orderProductService;
        private readonly IMapper _mapper;
        private float price = 0;
        private readonly ObservableCollection<OrderProductDto> _orderProduct = new ObservableCollection<OrderProductDto>();

        public CashierOrder(IServiceProvider serviceProvider, IProductService productService, IDialogService dialogService, IOrderService orderService, IOrderProductService orderProductService, IMapper mapper, IAuthService authService)
        {
            _dialogService = dialogService;
            this.serviceProvider = serviceProvider;
            _productService = productService;
            InitializeComponent();
            Load();
            _orderService = orderService;
            _orderProductService = orderProductService;
            _mapper = mapper;
            _authService = authService;
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

                // Assuming ProductName is the correct property name in the Product class
                addProduct.ProductName = productDetails.ProductName;

                var existingProduct = _orderProduct.FirstOrDefault(p => p.ProductId == addProduct.ProductId);
                if (existingProduct != null)
                {
                    existingProduct.Quantity += addProduct.Quantity;
                    price += (float)(productPrice * addProduct.Quantity);
                    existingProduct.Price += (productPrice * addProduct.Quantity);
                }
                else
                {
                    addProduct.Price = (productPrice * addProduct.Quantity);
                    _orderProduct.Add(addProduct);
                    price += (float)(productPrice * addProduct.Quantity);
                   
                }

                MessageBox.Show($"Total Price: {price:C}", "Order Summary", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            LoadPriceCart();
            LoadCart();
        }


        private async void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgDataCart.SelectedItem is OrderProductDto selectedProduct)
            {
                var existingProduct = _orderProduct.FirstOrDefault(p => p.ProductId == selectedProduct.ProductId);
                if (existingProduct != null)
                {
                    var allProducts = await _productService.GetAllProducts();
                    var productDetails = allProducts.FirstOrDefault(p => p.Id == selectedProduct.ProductId);

                    if (productDetails is not null)
                    {
                        var productPrice = productDetails.Price;
                        price -= (float)(productPrice * existingProduct.Quantity);
                        selectedProduct.Price = Convert.ToDecimal(price);
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
            if (dgData.SelectedItem is Product selected)
            {
                product.ProductId = selected.Id;
                product.CreatedTimestamp = DateTime.Now;
                if (int.TryParse(txtQuantity.Text, out int quantity) && quantity > 0)
                {
                    product.ProductId = selected.Id;
                    product.CreatedTimestamp = DateTime.Now;
                    product.Quantity = quantity;
                    return product;
                }
                else
                {
                    MessageBox.Show("Quantity must be greater than zero.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a product before adding to the cart.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            return null;
        }
        private void Logout_Click(object sender, RoutedEventArgs e) {
            _authService.logOut();
            _dialogService.CloseDialog<CashierOrder>();
            _dialogService.ShowDialog<LoginPage>();
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private async void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPriceCart.Text) || string.IsNullOrWhiteSpace(txtAmountCCart.Text))
            {
                MessageBox.Show("Please ensure both Price and Amount are entered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (float.TryParse(txtPriceCart.Text, out float price) && float.TryParse(txtAmountCCart.Text, out float amount))
            {
                if (amount < price)
                {
                    MessageBox.Show("Amount provided is less than the total price. Please enter a valid amount.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                float change = amount - price;
                Change.Text = change.ToString("C");

                if (MessageBox.Show("Are you sure you want to complete the payment?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        //SaveOrderDetails(price, amount, change);
                        var order = new Order
                        {
                            TotalOrderPrice = decimal.Parse(price.ToString()),
                            CustomerPay = decimal.Parse(amount.ToString()),
                            CreatedTimestamp = DateTime.Now,
                            Status = "Completed",
                            Reason = "Payment Received"
                        };

                        var orderId = await _orderService.AddOrder(order);
                        foreach (var item in _orderProduct)
                        {
                            item.OrderId = orderId;
                            await _orderProductService.AddOrderProduct(_mapper.Map<OrderProduct>(item));
                            
                        }
                        PrintReceiptJob();
                        MessageBox.Show("Payment processed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                        ResetOrderData();

                        var dashboardCashier = serviceProvider.GetRequiredService<DashboardCashier>();
                        await dashboardCashier.LoadStatisticDashboardAsync();

                        ClearPaymentFields();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"An error occurred while processing the payment: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Invalid price or amount format. Please check the values entered.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void ResetOrderData()
        {
            _orderProduct.Clear();

            price = 0;

            txtPriceCart.Clear();
            txtAmountCCart.Clear();
            Change.Clear();

            dgDataCart.ItemsSource = null;
        }

        private void ClearPaymentFields()
        {
            txtPriceCart.Clear();
            txtAmountCCart.Clear();
            Change.Clear();
        }
        private async void Receipt_Click(object sender, RoutedEventArgs e)
        {
            // Use a simple task to run the print operation asynchronously
            await Task.Run(() =>
            {
                // Log to see if the method is being triggered
                Debug.WriteLine("Receipt_Click triggered");

                PrintReceiptJob();
            });
        }

        private void PrintReceiptJob()
        {
            try
            {
                // Log the entry into the job
                Debug.WriteLine("PrintReceiptJob started");

                // Now use the Dispatcher to ensure UI-related tasks are on the UI thread
                Dispatcher.Invoke(() =>
                {
                    // Log to see if Dispatcher is called
                    Debug.WriteLine("Dispatcher.Invoke called");

                    PrintDialog printDialog = new PrintDialog();

                    // Check if printDialog is initialized
                    if (printDialog == null)
                    {
                        Debug.WriteLine("PrintDialog is null");
                    }
                    else
                    {
                        Debug.WriteLine("PrintDialog initialized");
                    }

                    // Set up the print settings
                    printDialog.PrintTicket.PageOrientation = System.Printing.PageOrientation.Portrait;

                    // Create the visual content for the receipt
                    DrawingVisual drawingVisual = new DrawingVisual();
                    using (DrawingContext drawingContext = drawingVisual.RenderOpen())
                    {
                        PrintReceipt(drawingContext);  // Call the method to draw the receipt content
                    }

                    // Create a FixedDocument
                    FixedDocument fixedDocument = new FixedDocument();

                    // Create a FixedPage and set its dimensions
                    FixedPage fixedPage = new FixedPage();
                    fixedPage.Width = 600;
                    fixedPage.Height = 800;

                    // Create a VisualBrush to render the DrawingVisual
                    VisualBrush visualBrush = new VisualBrush(drawingVisual);

                    // Create a rectangle to hold the VisualBrush (it converts the DrawingVisual to a UIElement)
                    Rectangle rectangle = new Rectangle
                    {
                        Width = fixedPage.Width,
                        Height = fixedPage.Height,
                        Fill = visualBrush
                    };

                    // Add the rectangle to the FixedPage
                    fixedPage.Children.Add(rectangle);

                    // Add the FixedPage to the FixedDocument
                    PageContent pageContent = new PageContent();
                    ((IAddChild)pageContent).AddChild(fixedPage);
                    fixedDocument.Pages.Add(pageContent);

                    // Show print dialog and print the receipt
                    bool? result = printDialog.ShowDialog();  // Use ShowDialog instead of PrintDocument

                    if (result == true)
                    {
                        printDialog.PrintDocument(fixedDocument.DocumentPaginator, "Receipt");
                        Debug.WriteLine("Printing completed");
                    }
                    else
                    {
                        Debug.WriteLine("PrintDialog was canceled or failed.");
                    }
                });
            }
            catch (Exception ex)
            {
                // Handle exceptions, display an error message on the UI thread
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"Error during receipt printing: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }


        // Method to draw the content of the receipt
        [Obsolete]
        private void PrintReceipt(DrawingContext drawingContext)
        {
            double y = 20;
            int colWidth = 120;
            double marginLeft = 10;
            Typeface boldFont = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
            Typeface regularFont = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Regular, FontStretches.Normal);

            // Draw header (centered)
            string headerText = "T1 BaseCamp CoffeShop";
            FormattedText headerFormattedText = new FormattedText(
                headerText,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                boldFont,
                24,
                Brushes.Black
            );

            headerFormattedText.TextAlignment = TextAlignment.Center;

            drawingContext.DrawText(headerFormattedText, new Point((headerFormattedText.Width) / 2, y));
            y += 50;

            // Table headers
            string[] headers = { "ProductName", "Quantity", "Time", "Price" };
            for (int i = 0; i < headers.Length; i++)
            {
                drawingContext.DrawText(
                    new FormattedText(
                        headers[i],
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        boldFont,
                        16,
                        Brushes.Black
                    ),
                    new Point(marginLeft + i * colWidth, y)
                );
            }

            y += 35;

            double orderItemY = y;

            // Display each product in the order
            foreach (var item in _orderProduct)
            {
                // Print Product Name
                drawingContext.DrawText(
                    new FormattedText(
                        item.ProductName,
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        regularFont,
                        12,
                        Brushes.Black
                    ),
                    new Point(marginLeft + 0 * colWidth, orderItemY)
                );

                // Print Quantity
                drawingContext.DrawText(
                    new FormattedText(
                        item.Quantity?.ToString() ?? "0",
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        regularFont,
                        12,
                        Brushes.Black
                    ),
                    new Point(marginLeft + 1 * colWidth, orderItemY)
                );

                // Print Timestamp
                drawingContext.DrawText(
                    new FormattedText(
                        item.CreatedTimestamp?.ToString("g") ?? "-",
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        regularFont,
                        12,
                        Brushes.Black
                    ),
                    new Point(marginLeft + 2 * colWidth, orderItemY)
                );

                // Print Price
                drawingContext.DrawText(
                    new FormattedText(
                        ((double)(item.Price ?? 0)).ToString("C", System.Globalization.CultureInfo.InvariantCulture),
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        regularFont,
                        12,
                        Brushes.Black
                    ),
                    new Point(marginLeft + 3 * colWidth, orderItemY)
                );

                orderItemY += 15;
            }

            double footerY = orderItemY + 40;
            string footerText = $"Total: {price:C}\nAmount: {txtAmountCCart.Text}\nChange: {Change.Text}";
            drawingContext.DrawText(
                new FormattedText(
                    footerText,
                    System.Globalization.CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    regularFont,
                    16,
                    Brushes.Black
                ),
                new Point(marginLeft, footerY)
            );
        }



        private void LoadPriceCart()
        {
            txtPriceCart.Text = price.ToString();
        }

        private void LoadCart()
        {
            dgDataCart.ItemsSource = _orderProduct;
        }


        //private void Dashboard(object sender, RoutedEventArgs e)
        //{
        //    AdminDashboardPage dashboardPage = new AdminDashboardPage();
        //    MainFrame.Navigate(dashboardPage);
        //}
        private void txtAmountCCart_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                float totalPrice = float.Parse(txtPriceCart.Text);
                float amountGiven = float.Parse(txtAmountCCart.Text);
                float change = amountGiven - totalPrice;

                if (change < 0)
                {
                    Change.Text = "Insufficient Amount!";
                }
                else
                {
                    Change.Text = change.ToString("C");
                }
            }
            catch (Exception ex)
            {
                Change.Text = "Invalid Input";
            }
        }

        private void Dashboard(object sender, RoutedEventArgs e)
        {
            _dialogService.ShowDialog<DashboardCashier>();
        }
    }
}
