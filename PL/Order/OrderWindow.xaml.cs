using System;
using System.Windows;
using System.Windows.Input;
namespace PL.Order;

/// <summary>
/// Interaction logic for OrderWindow.xaml
/// </summary>
public partial class OrderWindow : Window
{
    //Varies access to the display layer
    BlApi.IBl bl = BlApi.Factory.Get();

    string? fromWindow;
    BO.Cart? cart;

    //dp order
    public BO.Order myOrder
    {
        get { return (BO.Order)GetValue(myOrderProperty); }
        set { SetValue(myOrderProperty, value); }
    }

    // Using a DependencyProperty as the backing store for myOrder.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty myOrderProperty =
        DependencyProperty.Register("myOrder", typeof(BO.Order), typeof(OrderWindow), new PropertyMetadata(null));

    /// <summary>
    /// Brings invitation details from
    /// </summary>
    /// <param name="id">Invitation ID for presentation</param>
    /// <param name="cart">To the extent that preserves the current basket</param>
    /// <param name="from">A local variable that keeps the page that time i</param>
    public OrderWindow(int id, BO.Cart cart = null, string from = null)
    {
        InitializeComponent();
        cart = new BO.Cart();
        this.cart = cart;
        fromWindow = from;

        //Pulling Invitation Information, if a problem is always the problem will be in the order of order ID
        try { myOrder = bl.order.GetOrderDetails(id); }
        catch (Exception)
        {
            MessageBox.Show("Sorry has a problem with a requested order of order", "ERROR:(", MessageBoxButton.OK);
            return;
        }
    }

    /// <summary>
    /// Status update according to order status
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnUpdate_Click(object sender, RoutedEventArgs e)
    {
        BO.Order temp;
        try
        {
            if (myOrder.status == (BO.OrderStatus)1)
            {
                temp = bl.order.OrderShippingUpdate(myOrder.ID);
                MessageBox.Show("Order Sended😊", "🍰", MessageBoxButton.OK);
            }
            else
            {
                temp = bl.order.OrderDeliveryUpdate(myOrder.ID);
                MessageBox.Show("Order Shippded😊", "🍰", MessageBoxButton.OK);
            }
            myOrder = new BO.Order();
            myOrder = temp;
        }
        catch (BO.InternalErrorException ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "OK?", MessageBoxButton.OK);
            return;
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK);
            return;
        }
    }

    /// <summary>
    /// Opens the previous page and closes the current one
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (fromWindow == "fromOrderTracking")
            new OrderTrackinkWindow(cart).Show();
        else
            new OrderForListWindow().Show();
        this.Close();
    }
}