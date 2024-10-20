using System;
using System.Collections.Generic;
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

namespace WpfApp.Admin
{
    /// <summary>
    /// Interaction logic for MainAdminWindows.xaml
    /// </summary>
    public partial class MainAdminWindows : Window
    {
        public MainAdminWindows()
        {
            InitializeComponent();

            this.Height = WPFAdminConstants.ADMIN_WINDOW_HEIGHT;
            this.Width = WPFAdminConstants.ADMIN_WINDOW_WIDTH;
            this.Title = WPFAdminConstants.ADMIN_WINDOW_TITLE;

            Loaded += Admin_Window_Loaded;
        }

        private void Admin_Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.DataContext is MainAdminWindowsViewModel viewModel)
            {
                viewModel.NavigatePageExecute(WPFAdminConstants.ADMIN_DASHBOARD_PAGE);
            }
        }
    }
}
