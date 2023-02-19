using Microsoft.VisualBasic;
using PL.MainPages;
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
        VideoControl.Play();
    }

    private void ShowProductsButton_Click(object sender, RoutedEventArgs e) 
    {
        new MainPages.MainManagerWindow().Show();
        this.Close();
    }

    private void btnCustomer_Click(object sender, RoutedEventArgs e)
    {
        new MainCustomerWindow().Show();
        this.Close();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new Homepage().Show();
        this.Close();
    }

 
}
