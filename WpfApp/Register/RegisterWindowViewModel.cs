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
using System.Diagnostics;
using WpfApp.Core.Dialog;

namespace WpfApp.Register
{
    public class RegisterWindowViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IDialogService _dialogService;

        private string _username;
        private string _password;
        private string _confirmPassword;

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
        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        public ICommand RegisterCommand { get; }
        public ICommand NavigateSignInCommand { get; }

        public RegisterWindowViewModel(IAuthService authService, IServiceProvider serviceProvider, IDialogService dialogService)
        {
            _authService = authService;
            _serviceProvider = serviceProvider;
            _dialogService = dialogService;
            RegisterCommand = new RelayCommand(ExecuteRegister, CanExecuteRegister);
            NavigateSignInCommand = new RelayCommand(ExecuteNavigateSignIn);
        }
        private void ExecuteNavigateSignIn(object obj)
        {
            _dialogService.CloseDialog<RegisterWindow>();
            _dialogService.ShowDialog<LoginPage>();
        }

        private async void ExecuteRegister(object obj)
        {
            SignUpDto signUpDto = new SignUpDto();
            signUpDto.Username = _username;
            signUpDto.Password = _password;
            signUpDto.ConfirmPassword = _confirmPassword;

            bool isSignUp = await _authService.signUp(signUpDto);

            if (isSignUp)
            {
                _dialogService.CloseDialog<RegisterWindow>();
                _dialogService.ShowDialog<LoginPage>();
            }
            else
            {
                MessageBox.Show("Error when signup");
            }
        }

        private bool CanExecuteRegister(object obj)
        {
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
