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

    /// <summary>
    /// ctor
    /// </summary>
    public LogInWindow()
    {
        CurrUser = new BO.Users();
        InitializeComponent();
    }

    /// <summary>
    /// Existing customer login attempt.
    /// </summary>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        string currPassword = CurrUser.Password;
        string currectPassword;
        if (CurrUser.Email == null || CurrUser.Password == null|| CurrUser.Password == ""|| CurrUser.Email == "")
            IsVisibility = true;
        else
            IsVisibility = false;

        if (!IsVisibility)
        {
            try
            {
                 CurrUser = bl.user.GetUser(CurrUser.Email);
            }
            catch (BO.InternalErrorException)
            {
                string message = "You are not in out system, Do you want to sign in?";
                string title = "Close Window";

                if (MessageBox.Show(message, title, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                {
                    new SignInWindow(BO.TypeOfUser.customer).Show();
                    this.Close();
                    return;
                }
                else
                {
                    this.Close();
                    return;
                }
            }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error); return; }

            if(CurrUser.Password!=currPassword)
            {
                BO.Users tmpUser = new BO.Users();
                tmpUser.Password = currPassword;
                tmpUser.Email = CurrUser.Email;
                CurrUser = tmpUser;
                MessageBox.Show("the password is uncurrect");

                return;
            }

            if (CurrUser.TypeOfUser == BO.TypeOfUser.manager)
            {
                new MainPages.MainManagerWindow().Show();
                this.Close();
            }
            else
            {
                BO.Cart cart = new BO.Cart
                {
                    CustomerName = CurrUser.Name,
                    CustomerAddress = CurrUser.Address,
                    CustomerEmail = CurrUser.Email,
                    items = new List<BO.OrderItem?>(),
                    TotalPrice = 0
                };
                new MainCustomerWindow(cart).Show();
                this.Close();
            }
        }
    }

    /// <summary>
    /// Sending to the system login page.
    /// </summary>
    private void btnSignInn_Click(object sender, RoutedEventArgs e)
    {
        new SignInWindow(BO.TypeOfUser.customer).Show();
        this.Close();
    }

    /// <summary>
    /// Return to the appropriate page and close the current page.
    /// </summary>
    private void txtBack_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }
}
