using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp.Core.Dialog
{
    public interface IDialogService
    {
        void ShowDialog<T>() where T : Window;
        void CloseDialog<T>() where T : Window;
        bool ShowConfirmationDialog(string message);
    }
}
