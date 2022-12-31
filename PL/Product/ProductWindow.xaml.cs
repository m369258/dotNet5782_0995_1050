using System;
using System.Windows;
namespace PL.Product;

/// <summary>
/// Interaction logic for ProductWindow.xaml
/// </summary>
public partial class ProductWindow : Window
{
    //A private variable to access the logic layer
    BlApi.IBl bl = BlApi.Factory.Get();

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
    public ProductWindow()
    {
        InitializeComponent();
        productCurrent = new BO.Product();
        cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));
        btnAddOrUpdateProduct.Content = "הוספה";
        txtID.IsEnabled = true;
    }

    /// <summary>
    /// Constructive action for product update status
    /// </summary>
    /// <param name="id">ID product</param>
    public ProductWindow(int id)
    {
        InitializeComponent();
        //Product request by ID from the logical layer
        try { productCurrent=bl.product.GetProduct(id); }
        catch (BO.InternalErrorException) { MessageBox.Show("מוצר לא קיים"); }

        //The name of the selected category
        cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));
        btnAddOrUpdateProduct.Content = "עידכון";

        //Locks the option to change ID
        txtID.IsEnabled = false;
    }

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
            MessageBox.Show("אנא מלא את כל השדות");
            return;
        }

        //Checking the correctness of the information received
        if (!int.TryParse(txtID.Text, out id)) { MessageBox.Show("מזהה לא חוקי"); return; } ;
        if (!double.TryParse(txtPrice.Text, out price)) { MessageBox.Show("מחיר לא חוקי"); return; } ;
        if (!int.TryParse(txtInStock.Text, out inStock)) { MessageBox.Show("כמות במלאי לא חוקית"); return; } ;

        //In case of addition, a product will be added to the logical layer
        if (btnAddOrUpdateProduct.Content.ToString() == "הוספה")
        {
            try
            {
                bl.product.AddProduct(new BO.Product()
                {
                    ID = id,
                    Name = txtName.Text,
                    Category = (BO.Category)(cbxCategory.SelectedItem),
                    Price = price,
                    InStock = inStock
                });
            }
            catch { MessageBox.Show("מוצר לא התווסף משום קלט לא חוקי");}
        }
        //In case of an update, the product will be updated to the logical layer
        else
        {
            try { bl.product.UpDateProduct(productCurrent); }
            catch { MessageBox.Show("מוצר לא התווסף משום קלט לא חוקי"); }
        }
        this.Close();
    }
}
