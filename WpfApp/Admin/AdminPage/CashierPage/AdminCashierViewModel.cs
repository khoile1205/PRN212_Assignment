using Core.Command;
using DataAccess.Models;
using Service.Services;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using WpfApp.Core.BaseViewModel;
using WpfApp.Utils;

namespace WpfApp.Admin.AdminPage.CashierPage
{
    public class AdminCashierViewModel : BaseViewModel
    {
        private readonly IUploadService _uploadService;
        private readonly IUserService _userService;
        private readonly IRoleService _roleService;

        public ObservableCollection<Role> Roles { get; } = new ObservableCollection<Role>();
        public ObservableCollection<User> ListCashier { get; } = new ObservableCollection<User>();

        private BitmapImage _cashierImage;
        public BitmapImage CashierImage
        {
            get => _cashierImage;
            set => SetProperty(ref _cashierImage, value);
        }

        private string _cashierImagePath;
        public string CashierImagePath
        {
            get => _cashierImagePath;
            set => SetProperty(ref _cashierImagePath, value);
        }

        private Role _selectedRole;
        public Role SelectedRole
        {
            get => _selectedRole;
            set => SetProperty(ref _selectedRole, value);
        }

        private User _selectedCashier;
        public User SelectedCashier
        {
            get => _selectedCashier;
            set
            {
                SetProperty(ref _selectedCashier, value);
                if (value != null)
                {
                    // Populate fields based on selected cashier for edit functionality
                    CashierImage = new BitmapImage(new Uri(value.Avatar));
                    CashierImagePath = value.Avatar;
                    SelectedRole = value.Role;
                    // Assume other properties like Username and Status are also mapped here
                }
            }
        }

        public ICommand ImportImageCommand { get; }
        public ICommand SaveImageCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand ClearCommand { get; }

        public AdminCashierViewModel(IUploadService uploadService, IUserService userService, IRoleService roleService)
        {
            _uploadService = uploadService ?? throw new ArgumentNullException(nameof(uploadService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _roleService = roleService ?? throw new ArgumentNullException(nameof(roleService));

            ImportImageCommand = new RelayCommand(ImportImage);
            AddCommand = new RelayCommand(async (_) => await AddCashierAsync());
            UpdateCommand = new RelayCommand(async (_) => await UpdateCashierAsync(), (_) => SelectedCashier != null);
            DeleteCommand = new RelayCommand(async (_) => await DeleteCashierAsync(), (_) => SelectedCashier != null);
            ClearCommand = new RelayCommand(ClearFields);

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
                // Handle error (e.g., show message to the user)
                Console.WriteLine($"Failed to load roles: {ex.Message}");
            }
        }

        private async Task LoadCashierAsync()
        {
            try
            {
                ListCashier.Clear();
                var listCashier = await _userService.GetListUserByRole(Service.Enums.AppEnums.ROLE_ENUMS.Cashier);
                foreach (var user in listCashier)
                {
                    ListCashier.Add(user);
                }
            }
            catch (Exception ex)
            {
                // Handle error (e.g., show message to the user)
                Console.WriteLine($"Failed to load cashiers: {ex.Message}");
            }
        }

        private void ImportImage(object _)
        {
            var (image, path) = _uploadService.UploadImage();
            if (image != null && path != null)
            {
                CashierImage = image;
                CashierImagePath = path;
            }
        }

        private async Task AddCashierAsync()
        {
            var newUser = new User
            {
                Role = SelectedRole,
                Avatar = CashierImagePath,
                CreatedTimestamp = DateTime.Now
                // Map other necessary properties, e.g., Username and Password
            };

            await _userService.AddUser(newUser);
            ListCashier.Add(newUser);
            ClearFields();
        }

    }
}
