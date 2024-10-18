using Core.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfApp.Dialog;

namespace WpfApp.Admin
{
    public class MainAdminWindowsViewModel : INotifyPropertyChanged
    {
        private readonly IServiceProvider serviceProvider;
        private readonly IDialogService dialogService;
        #region ICommand
        public ICommand LogoutCommand { get; }
        #endregion
        public MainAdminWindowsViewModel(IServiceProvider serviceProvider, IDialogService dialogService)
        {
            this.serviceProvider = serviceProvider;
            this.dialogService = dialogService;
            LogoutCommand = new RelayCommand(LogoutExecute);
        }

        private void LogoutExecute(object obj)
        {
            dialogService.CloseDialog<MainAdminWindows>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
