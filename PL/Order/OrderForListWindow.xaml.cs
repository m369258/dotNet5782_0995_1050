using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;

namespace PL.Order;

/// <summary>
/// Interaction logic for OrderForListWindow.xaml
/// </summary>
public partial class OrderForListWindow : Window
{
    //A private variable to access the logic layer
    BlApi.IBl bl = BlApi.Factory.Get();

    public ObservableCollection<BO.OrderForList?> orders
    {
        get { return (ObservableCollection<BO.OrderForList?>)GetValue(ordersProperty); }
        set { SetValue(ordersProperty, value); }
    }

    // Using a DependencyProperty as the backing store for orders.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ordersProperty =
        DependencyProperty.Register("orders", typeof(ObservableCollection<BO.OrderForList?>), typeof(OrderForListWindow), new PropertyMetadata(null));


    public OrderForListWindow()
    {
        InitializeComponent();
        var temp = bl.order.GetListOfOrders();
        orders = temp == null ? new() : new(temp);
    }

    private void DGProducts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
         int idOrder=((BO.OrderForList)((DataGrid)sender).CurrentItem).OrderID;

          new OrderWindow(idOrder).ShowDialog();
    }
}
