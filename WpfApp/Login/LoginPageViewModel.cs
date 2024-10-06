using Core.Command;
using Microsoft.IdentityModel.Tokens;
using Service.DTO;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace WpfApp.Login
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        private readonly IAuthService _authService;
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
        public LoginPageViewModel(IAuthService authService)
        {
            _authService = authService;
            LoginCommand = new RelayCommand(ExecuteLogin, CanExecuteLogin);
        }

        private async void ExecuteLogin(object obj)
        {
            LoginDto loginDto = new LoginDto();
            loginDto.Username = _username;
            loginDto.Password = _password;
            UserDto user = await _authService.login(loginDto);
            if (user != null)
            {
                MessageBox.Show(user.Id.ToString());
            }
            else
            {
                MessageBox.Show("Not pass");

            }
        }

        private bool CanExecuteLogin(object obj)
        {
            //return (!_username.IsNullOrEmpty() && !_password.IsNullOrEmpty());
            return true;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
