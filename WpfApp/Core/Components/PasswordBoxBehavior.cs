using System;
using System.Windows;
using System.Windows.Controls;

namespace WpfApp.Core.Components
{
    public static class PasswordBoxBehavior
    {
        public static readonly DependencyProperty BindablePasswordProperty =
            DependencyProperty.RegisterAttached("BindablePassword", typeof(string), typeof(PasswordBoxBehavior),
                new FrameworkPropertyMetadata(string.Empty, OnBindablePasswordChanged));

        public static string GetBindablePassword(DependencyObject obj)
        {
            return (string)obj.GetValue(BindablePasswordProperty);
        }

        public static void SetBindablePassword(DependencyObject obj, string value)
        {
            obj.SetValue(BindablePasswordProperty, value);
        }

        private static void OnBindablePasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PasswordBox passwordBox)
            {
                // Temporarily unhook the event to avoid recursion
                passwordBox.PasswordChanged -= PasswordBox_PasswordChanged;

                // Set the new password value from the bound property
                passwordBox.SetCurrentValue(e.Property, e.NewValue);
                // Call a method to set focus and ensure caret is at the end

                // Rehook the event handler
                passwordBox.PasswordChanged += PasswordBox_PasswordChanged;
            }
        }

        private static void PasswordBox_PasswordChanged(object sender  , RoutedEventArgs e)
        {
            if (sender is PasswordBox passwordBox)
            {
                SetBindablePassword(passwordBox, passwordBox.Password);
            }
        }
    }
}