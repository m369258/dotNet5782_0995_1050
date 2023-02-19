using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
namespace PL.Product;

/// <summary>
/// Interaction logic for ProductWindow.xaml
/// </summary>
public partial class ProductWindow : Window
{
    //A private variable to access the logic layer
    BlApi.IBl bl = BlApi.Factory.Get();

    //IEnumerable<BO.Category> categories;
    public bool IsItForEditing
    {
        get { return (bool)GetValue(IsItForEditingProperty); }
        set { SetValue(IsItForEditingProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsItForEditing.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsItForEditingProperty =
        DependencyProperty.Register("IsItForEditing", typeof(bool), typeof(ProductWindow), new PropertyMetadata(true));

    public BO.Product productCurrent
    {
        get { return (BO.Product)GetValue(productCurrentProperty); }
        set { SetValue(productCurrentProperty, value); }
    }

    // Using a DependencyProperty as the backing store for productCurrent.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty productCurrentProperty =
        DependencyProperty.Register("productCurrent", typeof(BO.Product), typeof(ProductWindow), new PropertyMetadata(null));

    /// <summary>
    /// A constructive action for the state of adding a product
    /// </summary>
    //public ProductWindow()
    //{
    //    InitializeComponent();
    //    productCurrent = new BO.Product();
    //    cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));   
    //    btnAddOrUpdateProduct.Content = "Add";
    //    txtID.IsEnabled = true;
    //}

    /// <summary>
    /// Constructive action for product update status
    /// </summary>
    /// <param name="id">ID product</param>
    public ProductWindow(int id = 0, string fromWindow = "")
    {
        if (fromWindow == "tt")
            IsItForEditing = false;
        InitializeComponent();
        // categories= Enum.GetValues(typeof(BO.Category));
        cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));
        
        //Product request by ID from the logical layer
        try { 
            if (id != 0)
            {
                productCurrent = bl.product.GetProduct(id);
                IsItForEditing = false;
            }
        }
        catch (BO.InternalErrorException ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK);
            return;
        }
        catch (BO.InvalidArgumentException ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK);
            return;
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }

        //The name of the selected category
        cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));
        //btnAddOrUpdateProduct.Content = "Update";

        //Locks the option to change ID
        //txtID.IsEnabled = false;
    }

    //public ProductWindow(int id, string fromWhichWindow)
    //{
    //    InitializeComponent();
    //    //Product request by ID from the logical layer
    //    try { productCurrent = bl.product.GetProduct(id); }
    //    catch (BO.InternalErrorException) { MessageBox.Show("Product does not exist"); }

    //    //The name of the selected category
    //    cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));

    //    //Locks the option to change ID
    //    txtID.IsEnabled = false;
    //    txtName.IsEnabled = false;
    //    cbxCategory.IsEnabled = false;
    //    txtPrice.IsEnabled = false;
    //    txtInStock.IsEnabled = false;
    //    btnAddOrUpdateProduct.Visibility = Visibility.Collapsed;
    //}

    /// <summary>
    /// A function that updates or adds a product
    /// </summary>
    /// <param name="sender">Add or update button</param>
    /// <param name="e">More information about the button</param>
    private void btnAddOrUpdateProduct_Click(object sender, RoutedEventArgs e)
    {
        int id, inStock;
        double price;
        //A message will be displayed if one of the fields is empty
        if (txtID.Text == "" || txtName.Text == "" || txtPrice.Text == "" || txtInStock.Text == "" || cbxCategory.SelectedIndex == -1)
        {
            MessageBox.Show("Please fill in all fields");
            return;
        }

        //Checking the correctness of the information received
        if (!int.TryParse(txtID.Text, out id)) { MessageBox.Show("Invalid ID"); return; };
        if (!double.TryParse(txtPrice.Text, out price)) { MessageBox.Show("Invalid price"); return; };
        if (!int.TryParse(txtInStock.Text, out inStock)) { MessageBox.Show("Invalid stock quantity"); return; };

        //In case of addition, a product will be added to the logical layer
        if (btnAddOrUpdateProduct.Content.ToString() == "Add")
        {
            try
            {
                bl.product.AddProduct(productCurrent);
            }
            catch { MessageBox.Show("Product not added due to invalid input"); }
        }
        //In case of an update, the product will be updated to the logical layer
        else
        {
            try { bl.product.UpDateProduct(productCurrent); }
            catch { MessageBox.Show("Product not added due to invalid input"); }
        }
        new ProductForListWindow().Show();
        this.Close();
    }
    private void txtBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        new ProductForListWindow().Show();
        this.Close();
    }
}



public class AddOrUpdate : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isVisible = (bool)value;
        if (isVisible)
        {
            return "Add"; //Visibility.Collapsed;
        }
        else
        {
            return "Update";
        }
    }

    //convert from target property type to source property type
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


