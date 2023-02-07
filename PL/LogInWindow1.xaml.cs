////using System;
////using System.Collections.Generic;
////using System.Linq;
////using System.Text;
////using System.Threading.Tasks;
////using System.Windows;
////using System.Windows.Controls;
////using System.Windows.Data;
////using System.Windows.Documents;
////using System.Windows.Input;
////using System.Windows.Media;
////using System.Windows.Media.Imaging;
////using System.Windows.Shapes;

////namespace PL
////{
////    /// <summary>
////    /// Interaction logic for LogInWindow.xaml
////    /// </summary>
////    public partial class LogInWindow : Window
////    {
////        public LogInWindow()
////        {
////            InitializeComponent();
////        }
////    }
////}


//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace PL;

///// <summary>
///// Interaction logic for LogInWindow.xaml
///// </summary>
//public partial class LogInWindow1 : Window
//{
//    /// <summary>
//    /// Bl object to have an access to the Bl functions
//    /// </summary>
//    private BlApi.IBl bl = BlApi.Factory.Get();
//    /// <summary>
//    /// ctor for log in window
//    /// </summary>
//    public LogInWindow()
//    {
//        InitializeComponent();
//    }
//    /// <summary>
//    /// show the right window by the email and password
//    /// </summary>
//    /// <param name="sender"></param>
//    /// <param name="e"></param>
//    private void Button_Click(object sender, RoutedEventArgs e)
//    {
//        BO.Users user = new BO.Users();
        
//        //try
//        //{
//            user = bl.user.GetUser(txtEmail.Text, txtPassword.Password);
//        //}
//        //catch (BO.BlMissingEntityException ex)
//        //{
//        //    lblIncorrect.Visibility = Visibility.Visible;
//        //    return;
//        //}
//        //catch (BO.BlDetailInvalidException ex)
//        //{
//        //    lblIncorrect.Visibility = Visibility.Visible;
//        //    return;
//        //}
//        //catch (Exception ex)
//        //{
//        //    lblIncorrect.Visibility = Visibility.Visible;
//        //    return;
//        //}
//        //open the right window du to position:
//        if (user.TypeOfUser == BO.TypeOfUser.manager)
//        {
//            this.Close();
//            new MainPages.MainManagerWindow(user.Name).ShowDialog();
//        }
//        else
//        {
//            BO.Cart cart = new BO.Cart
//            {
//                CustomerName = user.Name,
//                CustomerAddress = user.Address,
//                CustomerEmail = user.Email,
//                items = new List<BO.OrderItem?>(),
//                TotalPrice = 0
//            };
//            this.Close();
//            new MainCustomerWindow(cart).ShowDialog();
//        }
//    }
//    /// <summary>
//    /// close the window
//    /// </summary>
//    /// <param name="sender"></param>
//    /// <param name="e"></param>
//    private void btnClose_Click(object sender, RoutedEventArgs e) => this.Close();
//    /// <summary>
//    /// if the user changes it, don't show the error label
//    /// </summary>
//    /// <param name="sender"></param>
//    /// <param name="e"></param>
//    private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
//    {
//        lblIncorrect.Visibility = Visibility.Collapsed;
//    }
//    /// <summary>
//    /// if the user changes it, don't show the error label
//    /// </summary>
//    /// <param name="sender"></param>
//    /// <param name="e"></param>
//    private void txtPassword_KeyDown(object sender, KeyEventArgs e) => lblIncorrect.Visibility = Visibility.Collapsed;


//    /// <summary>
//    /// open sign-up window
//    /// </summary>
//    /// <param name="sender"></param>
//    /// <param name="e"></param>
//    // private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e) => new SignUpWindow(BO.Position.Customer).ShowDialog();


//}