using Core.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service.DTO;
using WpfApp.Dialog;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WpfApp.Admin;
using Service.Services.Abstraction;
using WpfApp.Register;

namespace WpfApp.Login
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDialogService _dialogService;

        private string _username;
        private string _password;
        private string _errorMessage;

        public string UserName
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged(nameof(UserName));
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set
            {
                _errorMessage = value;
                OnPropertyChanged(nameof(ErrorMessage));
            }
        }

        public ICommand LoginCommand { get; }
        public ICommand OpenRegisterCommand { get; }
        public LoginPageViewModel(IAuthService authService, IServiceProvider serviceProvider, IDialogService dialogService)
        {
            this._authService = authService;
            this._serviceProvider = serviceProvider;
            this._dialogService = dialogService;
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            OpenRegisterCommand = new RelayCommand(ExecuteRegister);
        }

        private async void ExecuteLogin(object obj)
        {
            LoginDto loginDto = new LoginDto();
            loginDto.Username = _username;
            loginDto.Password = _password;

            UserDto user = await _authService.login(loginDto);
            if (user != null)
            {
                _dialogService.ShowDialog<MainAdminWindows>(); // Show the AdminPage
                _dialogService.CloseDialog<LoginPage>();
            }
            else
            {
                MessageBox.Show("Not pass");
            }
        }

        private bool CanExecuteLogin(object obj)
        {
            return true;
        }

        public void ExecuteRegister(object obj)
        {
            _dialogService.ShowDialog<RegisterWindow>(); // Show the AdminPage
            _dialogService.CloseDialog<LoginPage>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
