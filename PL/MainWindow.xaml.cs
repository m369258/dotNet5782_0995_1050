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

    /// <summary>
    /// Existing customer login
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnCustomer_Click(object sender, RoutedEventArgs e)
    {
        new MainCustomerWindow().Show();
        this.Close();
    }

    /// <summary>
    /// Foreign customer entry
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new Homepage().Show();
        this.Close();
    }

    /// <summary>
    /// Admin login
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click_2(object sender, RoutedEventArgs e)
    {
        new MainPages.MainManagerWindow().Show();
        this.Close();
    }
}
