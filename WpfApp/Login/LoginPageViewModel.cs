using Core.Command;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Service.DTO;
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
using WpfApp.Core.Dialog;
using WpfApp.Core.BaseViewModel;
using WpfApp.Cashier;

namespace WpfApp.Login
{
    public class LoginPageViewModel : BaseViewModel
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
            set => SetProperty(ref _username, value);

        }
        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);

        }
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);

        }

        public ICommand LoginCommand { get; }
        public ICommand NavigateRegisterCommand { get; }

        public LoginPageViewModel(IAuthService authService, IServiceProvider serviceProvider, IDialogService dialogService)
        {
            this._authService = authService;
            this._serviceProvider = serviceProvider;
            this._dialogService = dialogService;
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
            NavigateRegisterCommand = new RelayCommand(ExecuteNavigateRegister);
        }

        private async void ExecuteLogin(object obj)
        {
            LoginDto loginDto = new LoginDto
            {
                Username = _username,
                Password = _password
            };

            // Attempt login
            UserDto user = await _authService.login(loginDto);

            if (user != null && user.Role != null)
            {
                // Check the user's role using Role.Name
                if (user.Role.Name == "Admin") // Check for Admin Role
                {
                    _dialogService.ShowDialog<MainAdminWindows>();
                }
                else if (user.Role.Name == "Cashier") // Check for Cashier Role
                {
                    _dialogService.ShowDialog<CashierOrder>();
                }
                else
                {
                    MessageBox.Show("User role not recognized.");
                }

                // Close the login dialog
                _dialogService.CloseDialog<LoginPage>();
            }
            else
            {
                MessageBox.Show("Invalid username or password. Please try again.");
            }
        }


        private bool CanExecuteLogin(object obj)
        {
            return true;
        }

        public void ExecuteNavigateRegister(object obj)
        {
            _dialogService.ShowDialog<RegisterWindow>(); // Show the AdminPage
            _dialogService.CloseDialog<LoginPage>();
        }
    }
}
