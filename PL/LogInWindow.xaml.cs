using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace PL;

/// <summary>
/// Interaction logic for LogInWindow.xaml
/// </summary>
public partial class LogInWindow : Window
{
    private BlApi.IBl bl = BlApi.Factory.Get();

    //dp
    public BO.Users CurrUser
    {
        get { return (BO.Users)GetValue(CurrUserProperty); }
        set { SetValue(CurrUserProperty, value); }
    }

    // Using a DependencyProperty as the backing store for CurrUser.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty CurrUserProperty =
        DependencyProperty.Register("CurrUser", typeof(BO.Users), typeof(LogInWindow), new PropertyMetadata(null));

    //dp
    public bool IsVisibility
    {
        get { return (bool)GetValue(IsVisibilityProperty); }
        set { SetValue(IsVisibilityProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsVisibility.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsVisibilityProperty =
        DependencyProperty.Register("IsVisibility", typeof(bool), typeof(LogInWindow), new PropertyMetadata(false));

    public LogInWindow()
    {
        CurrUser = new BO.Users();
        InitializeComponent();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (CurrUser.Email == null || CurrUser.Password == null)
            IsVisibility = true;
        else
            IsVisibility = false;

        if (!IsVisibility)
        {
            try
            {
                CurrUser = bl.user.GetUser(CurrUser.Email, CurrUser.Password);
            }
            catch (BO.InternalErrorException)
            {
                string message = "You are not in out system, Do you want to sign in?";
                string title = "Close Window";

                if (MessageBox.Show(message, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    this.Close();
                    new SignInWindow(BO.TypeOfUser.customer).ShowDialog();
                    return;
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            if (CurrUser.TypeOfUser == BO.TypeOfUser.manager)
            {
                this.Close();
                new MainPages.MainManagerWindow(CurrUser.Name).ShowDialog();
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
