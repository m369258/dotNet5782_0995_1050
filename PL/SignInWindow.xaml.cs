using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Interaction logic for SignInWindow.xaml
    /// </summary>
    public partial class SignInWindow : Window
    {
        private BlApi.IBl bl = BlApi.Factory.Get();

        public BO.Users user
        {
            get { return (BO.Users)GetValue(userProperty); }
            set { SetValue(userProperty, value); }
        }

        // Using a DependencyProperty as the backing store for user.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty userProperty =
            DependencyProperty.Register("user", typeof(BO.Users), typeof(Window), new PropertyMetadata(null));

        public SignInWindow(BO.TypeOfUser position)
        {
            user = new BO.Users();
            InitializeComponent();
            user.TypeOfUser = position;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "" || txtAddress.Text == "" || txtEmail.Text == "" || !checkEmail() ||
                txtPassword.Text == "" || txtConfirmPassword.Text == "")
            {
                if (txtName.Text == "")
                    txtName.BorderBrush = Brushes.Red;
                if (txtAddress.Text == "")
                    txtAddress.BorderBrush = Brushes.Red;
                if (txtEmail.Text == "")
                    txtEmail.BorderBrush = Brushes.Red;
                else if (!checkEmail())
                {
                    txtEmail.BorderBrush = Brushes.Red;
                    txtEmail.Text = "❌     Email Is Invalid";
                }
                if (txtPassword.Text == "")
                    txtPassword.BorderBrush = Brushes.Red;
                if (txtConfirmPassword.Text != txtPassword.Text)
                    txtConfirmPassword.BorderBrush = Brushes.Red;
                return;
            }

            try
            {
                bl.user.AddUser(user);
            }
            //catch (BO.BlDetailInvalidException ex)
            //{
            //    System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            //catch (BO.BlAlreadyExistsEntityException ex)
            //{
            //    System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            System.Windows.MessageBox.Show("You've successfully signed up!😊", "💍", MessageBoxButton.OK);
            this.Close();
        }





        /// <summary>
        /// check if the email is valid
        /// </summary>
        /// <returns></returns>
        private bool checkEmail()
        {
            string email = txtEmail.Text;
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(email);
            return match.Success;
        }
    }
}
