using PL.Product;
using System;
using System.Collections.ObjectModel;
using System.Windows;
namespace PL.Order;

/// <summary>
/// Interaction logic for CatalogWindow.xaml
/// </summary>
public partial class CatalogWindow : Window
{
    BlApi.IBl bl = BlApi.Factory.Get();

    public ObservableCollection<BO.ProductItem?> ProductItem
    {
        get { return (ObservableCollection<BO.ProductItem?>)GetValue(ProductItemProperty); }
        set { SetValue(ProductItemProperty, value); }
    }

    // Using a DependencyProperty as the backing store for ProductItem.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProductItemProperty =
        DependencyProperty.Register("ProductItem", typeof(ObservableCollection<BO.ProductItem?>), typeof(CatalogWindow), new PropertyMetadata(null));

    public CatalogWindow()
    {
        InitializeComponent();
        //var caltalog = bl.product.GetCatalog();
        //lsvCategory.ItemsSource = caltalog;

        var temp = bl.product.GetCatalog();
        ProductItem = temp == null ? new() : new(temp);
        //Put all the categories
        cmbSelectCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));
    }

    private void cmbSelectCategory_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (cmbSelectCategory.SelectedIndex != 5)
        {
  BO.Category currCategorySelect = (BO.Category)cmbSelectCategory.SelectedItem;
        // ProductListview.ItemsSource = bl.product.GetListOfProducts((int)currCategorySelect);

        var temp = bl.product.GetCatalog((int)currCategorySelect);
        ProductItem = temp == null ? new() : new(temp);
        }
          else
        {
            // ProductListview.ItemsSource = bl.product.GetListOfProducts();
            var temp = bl.product.GetCatalog();
            ProductItem = temp == null ? new() : new(temp);
        }
    }

    private void lsvCategory_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
    {
        if (lsvCatlog.SelectedIndex != -1)
        {
            new ProductWindow(((BO.ProductItem)lsvCatlog.SelectedItem).ProductID, "FromCatalogWindow").Show();
            lsvCatlog.SelectedIndex = -1;
            cmbSelectCategory.SelectedIndex = 5;
            // ProductListview.ItemsSource = bl.product.GetListOfProducts();
            var temp = bl.product.GetCatalog();
            ProductItem = temp == null ? new() : new(temp);
        }

    }

    private void btnCompleteOrder_Click(object sender, RoutedEventArgs e)
    {

    }
}