using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace PL.Order;

/// <summary>
/// Interaction logic for OrderTrackinkWindow.xaml
/// </summary>
public partial class OrderTrackinkWindow : Window
{
    BlApi.IBl bl = BlApi.Factory.Get();
    string email;

    public ObservableCollection<BO.OrderTracking?> OrdersID
    {
        get { return (ObservableCollection<BO.OrderTracking?>)GetValue(OrdersIDProperty); }
        set { SetValue(OrdersIDProperty, value); }
    }

    // Using a DependencyProperty as the backing store for OrdersID.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrdersIDProperty =
        DependencyProperty.Register("OrdersID", typeof(ObservableCollection<BO.OrderTracking?>), typeof(OrderTrackinkWindow), new PropertyMetadata(null));

    //dp
    public BO.OrderTracking? tracking
    {
        get { return (BO.OrderTracking?)GetValue(trackingProperty); }
        set { SetValue(trackingProperty, value); }
    }

    // Using a DependencyProperty as the backing store for tracking.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty trackingProperty =
        DependencyProperty.Register("tracking", typeof(BO.OrderTracking), typeof(OrderTrackinkWindow), new PropertyMetadata(null));



    private BO.Cart cart;


    /// <summary>
    /// If it is a manager login, it will bring all the orders, otherwise it will bring the customer's orders
    /// </summary>
    public OrderTrackinkWindow(BO.Cart currCart=null)//!
    {
        InitializeComponent();
        this.email = currCart?.CustomerEmail;
        cart= currCart;

        try
        {
            if (email != ""&&email!=null)   //show user's orders
            {
                var temp = bl.order.OrdersOfUsers(email);
                OrdersID = temp == null ? new() : new(temp);
            }
            else    //show all orders (for manager)
            {
                var temp = bl.order.OrdersOfUsers();
                OrdersID = temp == null ? new() : new(temp);
            }
        }
        catch(BO.InternalErrorException) { MessageBox.Show("Sorry this order was not found");return; }
    }
    /// <summary>
    /// button for getting the order to track by id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnOrderDetails_Click(object sender, RoutedEventArgs e)
    {

        if (tracking == null)
            MessageBox.Show("To view items select an order");
        else
        {
            int orderid = tracking.ID;
            OrderWindow orderWindow = new OrderWindow(orderid);
            orderWindow.ShowDialog();
        }
    }

    /// <summary>
    /// Opens the page I came from
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        if (email == null)
            new MainPages.MainManagerWindow().Show();
        else
            new MainCustomerWindow(cart).Show();//!
        this.Close();
    }

    /// <summary>
    /// show tracking of the selected order in the combobox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        BO.OrderTracking selectedOrder = ((BO.OrderTracking)((ComboBox)sender).SelectedItem);
        tracking = bl.order.OrderTracking(selectedOrder.ID);
    }
}

