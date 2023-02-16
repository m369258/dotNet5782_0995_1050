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



    /// <summary>
    /// ctor for order tracking window
    /// </summary>
    public OrderTrackinkWindow(string email=null)
    {
        InitializeComponent();
        this.email = email;

        try
        {
            if (email != "")   //show user's orders
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
        catch { }
        //catch (BO.BlNullPropertyException ex)
        //{
        //    MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
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
    /// close window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.Close();

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

