using DataAccess.Models;
using Service.DTO.OrderDto;
using Service.Services.Abstraction;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;
using WpfApp.Admin.AdminPage.DashboardPage;
using WpfApp.Cashier;
using Microsoft.Data.SqlClient;
using System.Windows.Media;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Shapes;
using System.Diagnostics;


namespace WpfApp.Cashier
{
    public partial class CashierOrder : Window
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IProductService _productService;
        private float price = 0;
        private readonly ObservableCollection<OrderProductDto> _orderProduct = new ObservableCollection<OrderProductDto>();

        public CashierOrder(IServiceProvider serviceProvider, IProductService productService)
        {
            this.serviceProvider = serviceProvider;
            new ObservableCollection<OrderProductDto>();
            _productService = productService;
            InitializeComponent();
            Load();
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
            if (dgData.SelectedItem is Product selected)
            {
                product.ProductId = selected.Id;
                product.CreatedTimestamp = DateTime.Now;
                product.Quantity = int.Parse(string.IsNullOrEmpty(txtQuantity.Text) ? "0" : txtQuantity.Text);
            }
            return product;
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
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
                        SaveOrderDetails(price, amount, change);

                        MessageBox.Show("Payment processed successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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


        private void SaveOrderDetails(float totalPrice, float amountPaid, float change)
        {
            string connectionString = "Data Source=localhost;Initial Catalog=PRN211_Assignment;User ID=sa;Password=Cunhai123;Encrypt=True;Trust Server Certificate=True";

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    DateTime currentTimestamp = DateTime.Now;

                    string query = "INSERT INTO Orders (total_order_price, customer_pay, status, reason, created_timestamp, updated_timestamp) " +
                                   "VALUES (@totalOrderPrice, @customerPay, @status, @reason, @createdTimestamp, @updatedTimestamp)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@totalOrderPrice", totalPrice);
                        command.Parameters.AddWithValue("@customerPay", amountPaid);
                        command.Parameters.AddWithValue("@status", "Completed");
                        command.Parameters.AddWithValue("@reason", "Payment received");
                        command.Parameters.AddWithValue("@createdTimestamp", currentTimestamp);
                        command.Parameters.AddWithValue("@updatedTimestamp", currentTimestamp);

                        command.ExecuteNonQuery();
                    }

                    MessageBox.Show("Order has been saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while saving the order: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                    fixedPage.Width = 600;  // Adjust the width as necessary
                    fixedPage.Height = 800; // Adjust the height as necessary

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

                    // Log to check if we're at the print step
                    Debug.WriteLine("About to show PrintDialog");

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
        private void PrintReceipt(DrawingContext drawingContext)
        {
            // Setup layout values
            double y = 20;
            int colWidth = 120; // Increased column width for readability
            double marginLeft = 10;
            Typeface boldFont = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Bold, FontStretches.Normal);
            Typeface regularFont = new Typeface(new FontFamily("Arial"), FontStyles.Normal, FontWeights.Regular, FontStretches.Normal);

            // Draw header (centered)
            string headerText = "MarcoMan's Cafe Shop";
            FormattedText headerFormattedText = new FormattedText(
                headerText,
                System.Globalization.CultureInfo.InvariantCulture,
                FlowDirection.LeftToRight,
                boldFont,
                24, // Increased font size for the header
                Brushes.Black
            );
            drawingContext.DrawText(headerFormattedText, new Point((600 - headerFormattedText.Width) / 2, y));
            y += 50; // Increased spacing after header

            // Table headers
            string[] headers = { "OrderID", "ProductName", "Quantity", "Time", "Price" };
            for (int i = 0; i < headers.Length; i++)
            {
                drawingContext.DrawText(
                    new FormattedText(
                        headers[i],
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        boldFont,
                        16, // Increased font size for headers
                        Brushes.Black
                    ),
                    new Point(marginLeft + i * colWidth, y)
                );
            }

            y += 35; // Increased spacing after table headers

            // Display each product in the order
            foreach (var item in _orderProduct)
            {
                drawingContext.DrawText(
                    new FormattedText(
                        item.OrderId?.ToString() ?? "-",
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        regularFont,
                        12,
                        Brushes.Black
                    ),
                    new Point(marginLeft, y)
                );
                drawingContext.DrawText(
                    new FormattedText(
                        item.ProductName,
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        regularFont,
                        12,
                        Brushes.Black
                    ),
                    new Point(marginLeft + colWidth, y)
                );
                drawingContext.DrawText(
                    new FormattedText(
                        item.Quantity?.ToString() ?? "0",
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        regularFont,
                        12,
                        Brushes.Black
                    ),
                    new Point(marginLeft + 2 * colWidth, y)
                );
                drawingContext.DrawText(
                    new FormattedText(
                        item.CreatedTimestamp?.ToString("g") ?? "-",
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        regularFont,
                        12,
                        Brushes.Black
                    ),
                    new Point(marginLeft + 3 * colWidth, y)
                );
                drawingContext.DrawText(
                    new FormattedText(
                        ((double)(item.Price ?? 0)).ToString("C", System.Globalization.CultureInfo.InvariantCulture),
                        System.Globalization.CultureInfo.InvariantCulture,
                        FlowDirection.LeftToRight,
                        regularFont,
                        12,
                        Brushes.Black
                    ),
                    new Point(marginLeft + 4 * colWidth, y)
                );

                y += 5; // Increased spacing between items
            }

            // Print the footer with total price and change
            y += 40; // Extra space before footer
            string footerText = $"Total: {price:C}\nAmount: {txtAmountCCart.Text}\nChange: {Change.Text}";
            drawingContext.DrawText(
                new FormattedText(
                    footerText,
                    System.Globalization.CultureInfo.InvariantCulture,
                    FlowDirection.LeftToRight,
                    regularFont,
                    16, // Slightly larger font size for footer
                    Brushes.Black
                ),
                new Point(marginLeft, y)
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



    }
}
