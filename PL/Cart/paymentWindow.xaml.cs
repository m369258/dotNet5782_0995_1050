using System;
using System.Text.RegularExpressions;
using System.Windows;
namespace PL.Cart;

/// <summary>
/// Interaction logic for paymentWindow.xaml
/// </summary>
public partial class paymentWindow : Window
{
    private BlApi.IBl bl = BlApi.Factory.Get();

    //dp
    public BO.Cart MyCart
    {
        get { return (BO.Cart)GetValue(MyCartProperty); }
        set { SetValue(MyCartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyCart.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyCartProperty =
        DependencyProperty.Register("MyCart", typeof(BO.Cart), typeof(paymentWindow), new PropertyMetadata(null));

    public paymentWindow(BO.Cart getCart)
    {
        InitializeComponent();
        MyCart = getCart;
    }

    private void Button_Click(object sender, RoutedEventArgs e) => this.Close();


    /// <summary>
    /// Completing an order and throwing appropriate exceptions
    /// </summary>
    /// <param name="sender">button</param>
    /// <param name="e"></param>
    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        if (MyCart.CustomerAddress == null || MyCart.CustomerName == null || MyCart.CustomerEmail == null)
        {
            MessageBox.Show("pleas fill all of the fields", "OK?", MessageBoxButton.OK);
            return;
        }
            if (!checkEmail())
        {
            MessageBox.Show("email invalid", "OK?", MessageBoxButton.OK);
            return;
        }

        //Saving an order in the system
        try { bl.cart.MakeAnOrder(MyCart); }

        //One of the items is missing or incorrect
        catch (BO.InvalidArgumentException ex) { MessageBox.Show("Your" + ex.Message);return; }

        //The basket is empty or one of the products does not exist
        catch (BO.InternalErrorException ex) { MessageBox.Show("pay attention"+ex.Message);return; }

        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error); return; }

        //If everything is in order, we will inform the customer   
        MessageBox.Show("Your order is on its way...😊");
        
    }
    private bool checkEmail()
    {
        string email = MyCart.CustomerEmail;
        Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        Match match = regex.Match(email);
        return match.Success;
    }

}
