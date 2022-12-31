using PL.Product;
using System.Windows;
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

    public OrderWindow()
    {
        InitializeComponent();
    }
    public OrderWindow(int id)
    {
        InitializeComponent();
        try { myOrder = bl.order.GetOrderDetails(id); }
        catch  { MessageBox.Show("בעיה בהזמנה"); }
    }

    private void btnUpdate_Click(object sender, RoutedEventArgs e)
    {
        if (myOrder.status == (BO.OrderStatus)2) {
            try { bl.order.OrderDeliveryUpdate(myOrder.ID); }
            catch (BO.InternalErrorException ex) { MessageBox.Show(ex.Message); }
        }
        else
        {
            try { bl.order.OrderShippingUpdate(myOrder.ID); }
            catch { MessageBox.Show("יש בעיה בהזמנה"); }
        }
         
    }
}
