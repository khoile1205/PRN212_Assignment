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

namespace WpfApp.Dialog
{
    public class DialogService : IDialogService
    {
        private readonly IServiceProvider _serviceProvider;

        public DialogService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private static bool IsWindowOpen<T>(string windowName = "") where T : Window
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is T && (string.IsNullOrEmpty(windowName) || window.Name == windowName))
                {
                    return true;
                }
            }
            return false;
        }

        public void ShowDialog<T>() where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            window.Show();
        }

        public void CloseDialog<T>() where T : Window
        {
            var window = _serviceProvider.GetRequiredService<T>();
            Debug.WriteLine(IsWindowOpen<T>());
            Application.Current.Dispatcher.InvokeAsync(() => window.Close());
        }
    }
}
