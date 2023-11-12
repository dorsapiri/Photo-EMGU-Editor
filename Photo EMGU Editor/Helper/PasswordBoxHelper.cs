using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace Photo_EMGU_Editor.Helper
{
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty BoundPasswordProperty =
            DependencyProperty.RegisterAttached("BoundPassword", typeof(SecureString), typeof(PasswordBoxHelper),
                                                new FrameworkPropertyMetadata(null, OnBoundPasswordChanged));

        public static string GetBoundPassword(DependencyObject d)
        {
            return (string)d.GetValue(BoundPasswordProperty);
        }

        public static void SetBoundPassword(DependencyObject d, SecureString value)
        {
            d.SetValue(BoundPasswordProperty, value);
        }

        private static void OnBoundPasswordChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var passwordBox = d as PasswordBox;
            if (passwordBox == null)
                return;

            passwordBox.PasswordChanged -= PasswordBoxPasswordChanged;

            if (e.NewValue != null)
            {
                passwordBox.PasswordChanged += PasswordBoxPasswordChanged;
            }
        }

        private static void PasswordBoxPasswordChanged(object sender, RoutedEventArgs e)
        {
            var passwordBox = sender as PasswordBox;
            if (passwordBox == null)
                return;

            SetBoundPassword(passwordBox, passwordBox.SecurePassword);
        }
        static PasswordBoxHelper()
        {
            // This constructor is required for XAML to work properly
        }
    }
}
