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

        public SignInWindow(BO.TypeOfUser possion)
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (txtName.Text == "" || txtAddress.Text == "" || txtEmail.Text == "" || !checkEmail() ||
                txtPassword.Text == "" || txtConfirmPassword.Text == "")
            {
                if(txtName.Text == "")
                    txtName.BorderBrush = Brushes.Red;
                if(txtAddress.Text == "")
                    txtAddress.BorderBrush = Brushes.Red;
                if(txtEmail.Text=="")
                    txtEmail.BorderBrush = Brushes.Red;
                else if (!checkEmail())
                {
                    txtEmail.BorderBrush = Brushes.Red;
                    txtEmail.Text = "❌     Email Is Invalid";
                }
                if(txtPassword.Text=="")
                    txtPassword.BorderBrush = Brushes.Red;
                if(txtConfirmPassword.Text!=txtPassword.Text)
                    txtConfirmPassword.BorderBrush = Brushes.Red;


                //try
                //{
                //    bl.user.AddUser(user);
                //}

            }
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
