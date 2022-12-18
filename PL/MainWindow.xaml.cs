using PL.Product;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace PL;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private BlApi.IBl bl = new BlApi.Bl();
    public System.Windows.Media.ImageSource Icon { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        Uri iconUri = new Uri("pack://application:,,,/WPFIcon2.ico", UriKind.RelativeOrAbsolute);
        this.Icon = BitmapFrame.Create(iconUri);

    }

    private void ShowProductsButton_Click(object sender, RoutedEventArgs e) => new ProductForListWindow().Show();
}
