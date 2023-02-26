using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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

    /// <summary>
    /// Imports the display layer all orders and presents them
    /// </summary>
    public OrderForListWindow()
    {
        InitializeComponent();
        var temp = bl.order.GetListOfOrders();
        orders = temp == null ? new() : new(temp);
    }

    /// <summary>
    /// Opens the details of the selected order
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DGProducts_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        int idOrder = ((BO.OrderForList)((DataGrid)sender).CurrentItem).OrderID;
        new OrderWindow(idOrder).Show();
        this.Close();
    }

    /// <summary>
    /// Opens the page I came from and closes the current one
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        new MainPages.MainManagerWindow().Show();
        this.Close();
    }
}
