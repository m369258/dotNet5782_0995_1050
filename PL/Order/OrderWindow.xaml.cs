using System.Globalization;
using System;
using System.Windows;
using System.Windows.Data;
using BO;
using PL.Product;
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

    private void btnUpdate_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (myOrder.status == (BO.OrderStatus)2)
            {
                bl.order.OrderDeliveryUpdate(myOrder.ID);
            }
            else
            {
                bl.order.OrderShippingUpdate(myOrder.ID);
            }
        }
        catch (BO.InternalErrorException ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "bbbb:(", MessageBoxButton.OK);
            return;
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "aaaa:(", MessageBoxButton.OK);
            return;
        }
    }
    private void txtBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        new OrderForListWindow().Show();
        this.Close();
    }
}



