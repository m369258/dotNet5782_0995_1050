using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL
{
    /// <summary>
    /// Interaction logic for LogInWindow.xaml
    /// </summary>
    public partial class LogInWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();
        public LogInWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            BO.Users currUser = new BO.Users();
            try { currUser = bl.user.GetUser(txtEmail.Text, txtPassword.Text); }
            catch {
                string message = "You are not in out system, Do you want to sign in?";
                string title = "Close Window";

                if (MessageBox.Show(message, title, MessageBoxButton.YesNo)==MessageBoxResult.Yes)
                {
                    this.Close();
                    new SignInWindow(BO.TypeOfUser.customer).Show();
                    return;
                }
                else
                {
                    this.Close(); 
                    return;
                }
            }

            if (currUser.TypeOfUser == BO.TypeOfUser.manager)
            {
                this.Close();
                new MainPages.MainManagerWindow(currUser.Name).Show();
            }
            else
            {
                this.Close();
                BO.Cart cart = new BO.Cart
                {
                    CustomerName = currUser.Name,
                    CustomerAddress = currUser.Address,
                    CustomerEmail = currUser.Email,
                    items = new List<BO.OrderItem?>(),
                    TotalPrice = 0
                };
                new MainCustomerWindow(cart).Show();
            }
        }

        private void btnSignIn_Click(object sender, RoutedEventArgs e)
        {
            new SignInWindow(BO.TypeOfUser.customer).Show();
        }

        private void btnSignInn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            new SignInWindow(BO.TypeOfUser.customer).Show();
        }
    }
}
