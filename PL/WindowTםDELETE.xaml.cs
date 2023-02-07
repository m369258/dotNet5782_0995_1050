



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

namespace PL;

/// <summary>
/// Interaction logic for SignUpWindow.xaml
/// </summary>
public partial class WindowTםDELETE : Window
{
    /// <summary>
    /// Bl object to have an access to the Bl functions
    /// </summary>
    private BlApi.IBl bl = BlApi.Factory.Get();
    //dp
    public BO.Users user
    {
        get { return (BO.Users)GetValue(userProperty); }
        set { SetValue(userProperty, value); }
    }

    // Using a DependencyProperty as the backing store for prod.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty userProperty =
        DependencyProperty.Register("user", typeof(BO.Users), typeof(Window), new PropertyMetadata(null));
    /// <summary>
    /// ctor for sign up window
    /// </summary>
    public WindowTםDELETE(BO.TypeOfUser position)
    {
        user = new BO.Users();
        InitializeComponent();
        user.TypeOfUser = position;
    }
    /// <summary>
    /// button for signing up
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnSignUp_Click(object sender, RoutedEventArgs e)
    {
        //for the binding:
        nameTextBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        emailTextBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        addressTextBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        passwordTextBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();

        // checking all of the possible problems:
        if (nameTextBox.Text == ""
            || addressTextBox.Text == ""
            || emailTextBox.Text == "" || !checkEmail()
            || passwordTextBox.Text == ""
            || passwordCheckTextBox.Text == "" || passwordCheckTextBox.Text != passwordTextBox.Text)
        {
            //taking care of any problematic case: if there's a problem, color the border in red+show a message 
            if (nameTextBox.Text == "")
            {
                nameTextBox.BorderBrush = Brushes.Red;
                lblErrorName.Visibility = Visibility.Visible;
            }
            if (addressTextBox.Text == "")
            {
                addressTextBox.BorderBrush = Brushes.Red;
                lblErrorAddress.Visibility = Visibility.Visible;
            }
            if (emailTextBox.Text == "")
            {
                emailTextBox.BorderBrush = Brushes.Red;
                lblErrorEmail.Visibility = Visibility.Visible;
            }
            else if (!checkEmail())
            {
                emailTextBox.BorderBrush = Brushes.Red;
                lblErrorEmail.Content = "❌     Email Is Invalid";
                lblErrorEmail.Visibility = Visibility.Visible;
            }
            if (passwordTextBox.Text == "")
            {
                passwordTextBox.BorderBrush = Brushes.Red;
                lblErrorPassword.Visibility = Visibility.Visible;
            }
            if (passwordCheckTextBox.Text != passwordTextBox.Text)
            {
                passwordCheckTextBox.BorderBrush = Brushes.Red;
                lblErrorConfirm.Visibility = Visibility.Visible;
            }
            else if (passwordCheckTextBox.Text == "")
            {
                passwordCheckTextBox.BorderBrush = Brushes.Red;
                lblErrorConfirm.Content = "❌     Confirm Password Field Is Required";
                lblErrorConfirm.Visibility = Visibility.Visible;
            }
            // let the user change his mistake:
            return;
        }
        //add a user
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
        string email = emailTextBox.Text;
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        return match.Success;
    }
    /// <summary>
    /// close window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.Close();
    /// <summary>
    /// if there was a mistake, when the user is typing again remove the messages and red color
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        nameTextBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();

        if (nameTextBox.BorderBrush == Brushes.Red)
        {
            nameTextBox.BorderBrush = Brushes.DimGray;
            lblErrorName.Visibility = Visibility.Hidden;
        }
    }
    /// <summary>
    /// if there was a mistake, when the user is typing again remove the messages and red color
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void addressTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        addressTextBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();

        if (addressTextBox.BorderBrush == Brushes.Red)
        {
            addressTextBox.BorderBrush = Brushes.DimGray;
            lblErrorAddress.Visibility = Visibility.Hidden;
        }
    }
    /// <summary>
    /// if there was a mistake, when the user is typing again remove the messages and red color
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void emailTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        emailTextBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();

        if (emailTextBox.BorderBrush == Brushes.Red)
        {
            emailTextBox.BorderBrush = Brushes.DimGray;
            lblErrorEmail.Visibility = Visibility.Hidden;
        }
    }
    /// <summary>
    /// if there was a mistake, when the user is typing again remove the messages and red color
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void passwordTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        passwordTextBox.GetBindingExpression(System.Windows.Controls.TextBox.TextProperty).UpdateSource();
        if (passwordTextBox.BorderBrush == Brushes.Red)
        {
            passwordTextBox.BorderBrush = Brushes.DimGray;
            lblErrorPassword.Visibility = Visibility.Hidden;
        }
    }
    /// <summary>
    /// if there was a mistake, when the user is typing again remove the messages and red color
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void passwordCheckTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (passwordCheckTextBox.BorderBrush == Brushes.Red)
        {
            passwordCheckTextBox.BorderBrush = Brushes.DimGray;
            lblErrorConfirm.Visibility = Visibility.Hidden;
        }
    }
}
