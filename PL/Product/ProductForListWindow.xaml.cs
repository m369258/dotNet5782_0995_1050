using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
namespace PL.Product;

/// <summary>
/// Interaction logic for ProductForListWindow.xaml
/// </summary>
public partial class ProductForListWindow : Window
{
    //A private variable to access the logic layer
    BlApi.IBl bl = BlApi.Factory.Get();

    public ObservableCollection<BO.ProductForList?> Products
    {
        get { return (ObservableCollection<BO.ProductForList?>)GetValue(ProductsProperty); }
        set { SetValue(ProductsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for Products.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty ProductsProperty =
        DependencyProperty.Register("Products", typeof(ObservableCollection<BO.ProductForList?>), typeof(ProductForListWindow), new PropertyMetadata(null));

    /// <summary>
    /// A constructor action that initializes the controls
    /// </summary>
    public ProductForListWindow()
    {
        InitializeComponent();

        //Default all products
        cmxCategorySelector.SelectedIndex = 5;

        //Request the logical layer to bring all the products
        var temp=bl.product.GetListOfProducts();
        Products = temp == null ? new() : new(temp);
        //Put all the categories
        cmxCategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
    }

    /// <summary>
    /// List of products according to selected filter
    /// </summary>
    /// <param name="sender">comboBox of filter product</param>
    /// <param name="e">more information on comboBox of filter product</param>
    private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //Filter products by category if selected
         if (cmxCategorySelector.SelectedIndex != 5)
         {
          BO.Category currCategorySelect = (BO.Category)cmxCategorySelector.SelectedItem;
          var temp = bl.product.GetListOfProducts((int)currCategorySelect);
          Products = temp == null ? new() : new(temp);
        }
        else
        {
           var temp = bl.product.GetListOfProducts();
            Products = temp == null ? new() : new(temp);
        }
    }

    /// <summary>
    /// A function that opens a page for adding a product, and then updates the existing products
    /// </summary>
    /// <param name="sender">Button to add a product</param>
    /// <param name="e">Details on the add button</param>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        new ProductWindow().ShowDialog();

        //Refreshing the list of existing products
        var temp = bl.product.GetListOfProducts();
        Products = temp == null ? new() : new(temp);
    }

    /// <summary>
    /// When clicking on any product, a window will open to update the aforementioned product
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e">more information on </param>

    private void ProductListview_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
    {
        if (ProductListview.SelectedIndex != -1)
        {
            new ProductWindow(((BO.ProductForList)ProductListview.SelectedItem).ID).ShowDialog();
            ProductListview.SelectedIndex = -1;
            cmxCategorySelector.SelectedIndex = 5;
            // ProductListview.ItemsSource = bl.product.GetListOfProducts();
            var temp = bl.product.GetListOfProducts();
            Products = temp == null ? new() : new(temp);

        }
    }
}
