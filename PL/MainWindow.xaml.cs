using PL.Order;
using System.Windows;
namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private void ShowProductsButton_Click(object sender, RoutedEventArgs e) => new MainPages.MainManagerWindow().ShowDialog();

    private void category_Click(object sender, RoutedEventArgs e)
    {
        new Order.CatalogWindow().Show();
    }

    private void btnTrack_Click(object sender, RoutedEventArgs e)
    {
        string email =txtOrderId.Text;
        if (email==null)
            MessageBox.Show(" pleas insert your email:");
        else
            new OrderTrackinkWindow(email).Show(); ;

   }

    private void btnCustomer_Click(object sender, RoutedEventArgs e) =>new MainCustomerWindow().Show();
}
