using System;
using System.Windows;
using System.Windows.Input;

namespace PL.Order;

/// <summary>
/// Interaction logic for OrderWindow.xaml
/// </summary>
public partial class OrderWindow : Window
{
    BlApi.IBl bl = BlApi.Factory.Get();
    public BO.Order myOrder
    {
        get { return (BO.Order)GetValue(myOrderProperty); }
        set { SetValue(myOrderProperty, value); }
    }

    // Using a DependencyProperty as the backing store for myOrder.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty myOrderProperty =
        DependencyProperty.Register("myOrder", typeof(BO.Order), typeof(OrderWindow), new PropertyMetadata(null));

    public OrderWindow(int id)
    {
        InitializeComponent();
        try { myOrder = bl.order.GetOrderDetails(id); }
        catch (BO.InternalErrorException ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "ok:(", MessageBoxButton.OK);
            return;
        }
        catch (BO.InvalidArgumentException ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "OK:(", MessageBoxButton.OK);
            return;
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
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
        try
        {
            if (myOrder.status == (BO.OrderStatus)1)
            {
               BO.Order temp= bl.order.OrderShippingUpdate(myOrder.ID);
                myOrder = new BO.Order();
                myOrder = temp;
                MessageBox.Show("Order Sended😊", "🍰", MessageBoxButton.OK);
            }
            else
            {
               BO.Order temp= bl.order.OrderDeliveryUpdate(myOrder.ID);
                myOrder = new BO.Order();
                myOrder = temp;
                MessageBox.Show("Order Shippded😊", "🍰", MessageBoxButton.OK);
            }
        }
        catch (BO.InternalErrorException ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "🍰", MessageBoxButton.OK);
            return;
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "🍰", MessageBoxButton.OK);
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
        new OrderForListWindow().Show();
        this.Close();
    }
}