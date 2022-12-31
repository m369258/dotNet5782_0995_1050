using System.Windows;
namespace PL.Order;

/// <summary>
/// Interaction logic for CatalogWindow.xaml
/// </summary>
public partial class CatalogWindow : Window
{
    BlApi.IBl bl = BlApi.Factory.Get();
    public CatalogWindow()
    {
        InitializeComponent();
        var caltalog = bl.product.GetCatalog();
    }
}
