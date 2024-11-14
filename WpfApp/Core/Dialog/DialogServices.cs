using AutoMapper;
using DataAccess.Models;
using DataAccess.Repository;
using DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Service.DTO;
using Service.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp.Core.Components;

namespace WpfApp.Core.Dialog
{
    public class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void ShowDialog<T>() where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            window.Visibility = Visibility.Visible;
            Application.Current.MainWindow = window;
        }

        public void CloseDialog<T>() where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            window.Hide();
        }
    }
}
