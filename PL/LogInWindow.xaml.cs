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

namespace PL;

/// <summary>
/// Interaction logic for LogInWindow.xaml
/// </summary>
public partial class LogInWindow : Window
{
    /// <summary>
    /// Bl object to have an access to the Bl functions
    /// </summary>
    private BlApi.IBl bl = BlApi.Factory.Get();
    /// <summary>
    /// ctor for log in window
    /// </summary>
    public LogInWindow()
    {
        InitializeComponent();
    }
    /// <summary>
    /// show the right window by the email and password
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        BO.Users user = new BO.Users();
        
        try
        {
            user = bl.Users.GetByEmailAndPassword(txtEmail.Text, txtPassword.Password);
        }
        catch (BO.BlMissingEntityException ex)
        {
            lblIncorrect.Visibility = Visibility.Visible;
            return;
        }
        catch (BO.BlDetailInvalidException ex)
        {
            lblIncorrect.Visibility = Visibility.Visible;
            return;
        }
        catch (Exception ex)
        {
            lblIncorrect.Visibility = Visibility.Visible;
            return;
        }
        //open the right window du to position:
        if (user.Position == BO.Position.Manager)
        {
            this.Close();
            new ManagerWindow(user.Name).ShowDialog();
        }
        else
        {
            BO.Cart cart = new BO.Cart
            {
                CustomerName = user.Name,
                CustomerAddress = user.Address,
                CustomerEmail = user.Email,
                Items = new List<BO.OrderItem?>(),
                TotalPrice = 0
            };
            this.Close();
            new CustomerWindow(cart).ShowDialog();
        }
    }
    /// <summary>
    /// close the window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnClose_Click(object sender, RoutedEventArgs e) => this.Close();
    /// <summary>
    /// if the user changes it, don't show the error label
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtEmail_TextChanged(object sender, TextChangedEventArgs e)
    {
        lblIncorrect.Visibility = Visibility.Collapsed;
    }
    /// <summary>
    /// if the user changes it, don't show the error label
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtPassword_KeyDown(object sender, KeyEventArgs e) => lblIncorrect.Visibility = Visibility.Collapsed;


    /// <summary>
    /// open sign-up window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e) => new SignUpWindow(BO.Position.Customer).ShowDialog();


}