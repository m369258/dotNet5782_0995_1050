using PL.Order;
using PL.Product;
using System.Windows;
using System.Windows.Input;

namespace PL.MainPages;

/// <summary>
/// Interaction logic for MainManagerWindow.xaml
/// </summary>
public partial class MainManagerWindow : Window
{
    public MainManagerWindow()
    {
        InitializeComponent();
    }
    private void btnProducts_Click(object sender, RoutedEventArgs e)
    {
        new ProductForListWindow().Show();
        this.Close();
    }

    private void btnOrders_Click(object sender, RoutedEventArgs e)
    {
        new OrderForListWindow().Show();
        this.Close();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new OrderTrackinkWindow().Show();
        this.Close();
    }

    private void txtBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) 
    { 
        new MainWindow().Show();
        this.Close();
    }
}
