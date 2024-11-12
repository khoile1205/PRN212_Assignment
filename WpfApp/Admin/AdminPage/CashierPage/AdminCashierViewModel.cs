using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataAccess.Models;
using Service.Services.Abstraction;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using WpfApp.Utils;

namespace WpfApp.Admin.AdminPage.CashierPage
{
    public partial class AdminCashierViewModel : ObservableObject
    {
        private readonly IImageService _imageService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public ObservableCollection<Role> Roles { get; } = new();
        public ObservableCollection<User> ListCashier { get; } = new();
        public ObservableCollection<string> StatusOptions { get; } = new() { "Active", "Inactive" };

        [ObservableProperty]
        private BitmapImage? _cashierImage;

        [ObservableProperty]
        private string? _cashierImagePath;

        [ObservableProperty]
        private Role? _selectedRole;

        [ObservableProperty]
        private string? _selectedStatus;

        [ObservableProperty]
        private User? _selectedCashier;

        [ObservableProperty]
        private string? _username;

        [ObservableProperty]
        private string? _password;

        [ObservableProperty]
        private string? _status;

        public AdminCashierViewModel(IImageService imageService, IUserService userService, IRoleService roleService)
        {
            _imageService = imageService;
            _userService = userService;
            _roleService = roleService;

            InitializeViewModelAsync();
        }

        private async Task InitializeViewModelAsync()
        {
            await LoadRolesAsync();
            await LoadCashierAsync();
        }

        private async Task LoadRolesAsync()
        {
            try
            {
                Roles.Clear();
                var roles = await _roleService.GetList();
                foreach (var role in roles)
                {
                    Roles.Add(role);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load roles: {ex.Message}");
            }
        }

        private async Task LoadCashierAsync()
        {
            try
            {
                ListCashier.Clear();
                var listCashier = await _userService.GetListUsers();
                foreach (var user in listCashier)
                {
                    ListCashier.Add(user);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load cashiers: {ex.Message}");
            }
        }



        [RelayCommand]
        private void ImportImage()
        {
            try
            {
                var (image, path) = _imageService.UploadImage();
                if (image != null && path != null)
                {
                    CashierImage = image;
                    CashierImagePath = path;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to import image: {ex.Message}");
            }
        }

        [RelayCommand]
        private async Task AddCashierAsync()
        {
            if (!ValidateCashierInputs()) return;

            try
            {
                var newUser = new User
                {
                    Role = SelectedRole,
                    Avatar = CashierImagePath,
                    CreatedTimestamp = DateTime.Now,
                    Username = Username,
                    Password = Password,
                    Status = SelectedStatus
                };

                await _userService.AddUser(newUser);
                _imageService.SaveImage(CashierImage, CashierImagePath);
                await LoadCashierAsync();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to add cashier: {ex.Message}");
            }
        }

        [RelayCommand(CanExecute = nameof(CanUpdateOrDelete))]
        private async Task UpdateCashierAsync()
        {
            if (!ValidateCashierInputs() || SelectedCashier == null) return;

            try
            {
                SelectedCashier.Role = SelectedRole;
                SelectedCashier.Avatar = CashierImagePath;
                SelectedCashier.Username = Username;
                SelectedCashier.Status = SelectedStatus;

                await _userService.UpdateUser(SelectedCashier);
                await LoadCashierAsync();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to update cashier: {ex.Message}");
            }
        }

        [RelayCommand(CanExecute = nameof(CanUpdateOrDelete))]
        private async Task DeleteCashierAsync()
        {
            try
            {
                if (SelectedCashier == null)
                {
                    MessageBox.Show("Please select a cashier to delete.");
                    return;
                }

                await _userService.DeleteUser(SelectedCashier.Id);
                ListCashier.Remove(SelectedCashier);
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete cashier: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ClearFields()
        {
            try
            {
                SelectedCashier = null;
                CashierImage = null;
                CashierImagePath = string.Empty;
                SelectedRole = null;
                SelectedStatus = null; // Clear the status
                Username = string.Empty;
                Password = string.Empty; // Clear the password
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to clear fields: {ex.Message}");
            }
        }

        //[RelayCommand]
        //private void OnSelectedCashier()
        //{

        //}
        private bool CanUpdateOrDelete() => SelectedCashier != null;

        private bool ValidateCashierInputs()
        {
            if (string.IsNullOrEmpty(Username))
            {
                MessageBox.Show("Username is required.");
                return false;
            }

            if (string.IsNullOrEmpty(Password))
            {
                MessageBox.Show("Password is required.");
                return false;
            }

            if (SelectedRole == null)
            {
                MessageBox.Show("Role is required.");
                return false;
            }

            if (string.IsNullOrEmpty(SelectedStatus))
            {
                MessageBox.Show("Status is required.");
                return false;
            }

            //if (string.IsNullOrEmpty(CashierImagePath))
            //{
            //    MessageBox.Show("Image is required.");
            //    return false;
            //}

            return true;
        }
    }
}
