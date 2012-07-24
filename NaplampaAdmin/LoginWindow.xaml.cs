using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using NaplampaAdmin.NaplampaService;
using NaplampaAdmin.Util;

namespace NaplampaAdmin
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();

#if DEBUG
            UsernameTextBox.Text = "19000000";
            PasswordBox.Password = "JlG9OpM7";
#endif
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ServiceManager.SetCredentials(UsernameTextBox.Text, PasswordBox.Password);

            int personId = -1;
            if (!Int32.TryParse(UsernameTextBox.Text, out personId)) return;

            Person person = ServiceManager.NaplampaService.Login(personId, PasswordBox.Password);

            if (person == null) return;

            this.Visibility = Visibility.Hidden;
            GridWindow gridWindow = new GridWindow(person);
            gridWindow.Show();
        }

        private void ResetPassordButton_Click(object sender, RoutedEventArgs e)
        {
            int personId = -1;
            if (!Int32.TryParse(UsernameTextBox.Text, out personId)) return;

            ServiceManager.NaplampaService.ResetPassword(personId);
        }
    }
}
