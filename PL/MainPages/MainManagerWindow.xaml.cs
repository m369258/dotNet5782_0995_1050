using PL.Order;
using PL.Product;
using System.Windows;
namespace PL.MainPages;

/// <summary>
/// Interaction logic for MainManagerWindow.xaml
/// </summary>
public partial class MainManagerWindow : Window
{
    public string name { get; set; }

    public MainManagerWindow(string userName="")
    {
        InitializeComponent();
       this.name = userName;
    }
    private void btnProducts_Click(object sender, RoutedEventArgs e)
    {
        new ProductForListWindow().ShowDialog();
    }

    private void btnOrders_Click(object sender, RoutedEventArgs e)
    {
        new OrderForListWindow().ShowDialog();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new OrderTrackinkWindow().ShowDialog(); }
}
