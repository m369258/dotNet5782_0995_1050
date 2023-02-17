using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

namespace PL;

/// <summary>
/// Interaction logic for LogInWindow.xaml
/// </summary>
public partial class LogInWindow : Window
{


    public BO.Users CurrUser
    {
        get { return (BO.Users)GetValue(CurrUserProperty); }
        set { SetValue(CurrUserProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrUser.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrUserProperty =
        DependencyProperty.Register("CurrUser", typeof(BO.Users), typeof(LogInWindow), new PropertyMetadata(null));




    public bool IsFill
    {
        get { return (bool)GetValue(IsFillProperty); }
        set { SetValue(IsFillProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsFill.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsFillProperty =
        DependencyProperty.Register("IsFill", typeof(bool), typeof(LogInWindow), new PropertyMetadata(false));



    private BlApi.IBl bl = BlApi.Factory.Get();
    public LogInWindow()
    {
        InitializeComponent();

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (CurrUser == null || CurrUser.Email == "" || CurrUser.Password == "")
            IsFill = false;
        else
            IsFill = true;
        if(IsFill)
        {
            try { CurrUser = bl.user.GetUser(CurrUser.Email, CurrUser.Password); }
            catch
            {
                string message = "You are not in out system, Do you want to sign in?";
                string title = "Close Window";

                if (MessageBox.Show(message, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
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

            if (CurrUser.TypeOfUser == BO.TypeOfUser.manager)
            {
                this.Close();
                new MainPages.MainManagerWindow(CurrUser.Name).Show();
            }
            else
            {
                this.Close();
                BO.Cart cart = new BO.Cart
                {
                    CustomerName = CurrUser.Name,
                    CustomerAddress = CurrUser.Address,
                    CustomerEmail = CurrUser.Email,
                    items = new List<BO.OrderItem?>(),
                    TotalPrice = 0
                };
                new MainCustomerWindow(cart).Show();
            }


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

    private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
    {
        
    }
}
