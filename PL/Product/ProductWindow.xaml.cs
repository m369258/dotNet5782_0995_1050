//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;
//using System.IO;
//using Microsoft.Win32;
//using System.Globalization;
//namespace PL.Product;

///// <summary>
///// Interaction logic for ProductWindow.xaml
///// </summary>
//public partial class ProductWindow : Window
//{
//    //A private variable to access the logic layer
//    BlApi.IBl bl = BlApi.Factory.Get();

//    public BO.Product productCurrent
//    {
//        get { return (BO.Product)GetValue(productCurrentProperty); }
//        set { SetValue(productCurrentProperty, value); }
//    }

//    // Using a DependencyProperty as the backing store for productCurrent.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty productCurrentProperty =
//        DependencyProperty.Register("productCurrent", typeof(BO.Product), typeof(ProductWindow), new PropertyMetadata(null));

//    /// <summary>
//    /// A constructive action for the state of adding a product
//    /// </summary>
//    public ProductWindow()
//    {
//        InitializeComponent();
//        productCurrent = new BO.Product();
//        cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));
//        btnAddOrUpdateProduct.Content = "Add";
//        txtID.IsEnabled = true;
//    }

//    /// <summary>
//    /// Constructive action for product update status
//    /// </summary>
//    /// <param name="id">ID product</param>
//    public ProductWindow(int id)
//    {
//        InitializeComponent();
//        //Product request by ID from the logical layer
//        try { productCurrent = bl.product.GetProduct(id); }
//        catch (BO.InternalErrorException ex)
//        {
//            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK);
//            return;
//        }
//        catch (BO.InvalidArgumentException ex)
//        {
//            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK);
//            return;
//        }
//        catch (Exception ex)
//        {
//            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
//            return;
//        }

//        //The name of the selected category
//        cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));
//        btnAddOrUpdateProduct.Content = "Update";

//        //Locks the option to change ID
//        txtID.IsEnabled = false;
//    }

//    public ProductWindow(int id, string fromWhichWindow)
//    {
//        InitializeComponent();
//        //Product request by ID from the logical layer
//        try { productCurrent = bl.product.GetProduct(id); }
//        catch (BO.InternalErrorException) { MessageBox.Show("Product does not exist"); }

//        //The name of the selected category
//        cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));

//        //Locks the option to change ID
//        txtID.IsEnabled = false;
//        txtName.IsEnabled = false;
//        cbxCategory.IsEnabled = false;
//        txtPrice.IsEnabled = false;
//        txtInStock.IsEnabled = false;
//        btnAddOrUpdateProduct.Visibility = Visibility.Collapsed;
//    }

//    /// <summary>
//    /// A function that updates or adds a product
//    /// </summary>
//    /// <param name="sender">Add or update button</param>
//    /// <param name="e">More information about the button</param>
//    private void btnAddOrUpdateProduct_Click(object sender, RoutedEventArgs e)
//    {
//        int id, inStock;
//        double price;
//        //A message will be displayed if one of the fields is empty
//        if (txtID.Text == "" || txtName.Text == "" || txtPrice.Text == "" || txtInStock.Text == "" || cbxCategory.SelectedIndex == -1)
//        {
//            MessageBox.Show("Please fill in all fields");
//            return;
//        }

//        //Checking the correctness of the information received
//        if (!int.TryParse(txtID.Text, out id)) { MessageBox.Show("Invalid ID"); return; };
//        if (!double.TryParse(txtPrice.Text, out price)) { MessageBox.Show("Invalid price"); return; };
//        if (!int.TryParse(txtInStock.Text, out inStock)) { MessageBox.Show("Invalid stock quantity"); return; };
//        if (id > 100000 && id < 1000000) { MessageBox.Show("the id invalid");return; };

//        //In case of addition, a product will be added to the logical layer
//        if (btnAddOrUpdateProduct.Content.ToString() == "Add")
//        {
//            try
//            {
//                bl.product.AddProduct(productCurrent);
//            }
//            catch { MessageBox.Show("Product not added due to invalid input"); }
//        }
//        //In case of an update, the product will be updated to the logical layer
//        else
//        {
//            try { bl.product.UpDateProduct(productCurrent); }
//            catch { MessageBox.Show("Product not added due to invalid input"); }
//        }
//        new ProductForListWindow().Show();
//        this.Close();
//    }

//    private void txtBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
//    {
//        new ProductForListWindow().Show();
//        this.Close();
//    }




//    /// <summary>
//    /// let the manager select an image from the browser
//    /// </summary>
//    /// <param name="sender"></param>
//    /// <param name="e"></param>
//    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
//    {
//        OpenFileDialog openFileDialog = new OpenFileDialog();
//        if (openFileDialog.ShowDialog() == true)
//        {
//            pbx.Source = new BitmapImage(new Uri(openFileDialog.FileName));
//            productCurrent.Img = openFileDialog.FileName;

//        }
//    }

//    /// <summary>
//    /// close the current window
//    /// </summary>
//    /// <param name="sender"></param>
//    /// <param name="e"></param>
//   // private void Button_Click(object sender, RoutedEventArgs e) => this.Close();

//    /// <summary>
//    /// the function treats the things of the image
//    /// </summary>
//    private void treatImage()
//    {
//        if (productCurrent.Img != null)
//        {
//            string imageName = productCurrent.Img.Substring(productCurrent.Img.LastIndexOf("\\"));
//            if (!File.Exists(Environment.CurrentDirectory[..^4] + @"\pics\" + imageName))
//                File.Copy(productCurrent.Img, Environment.CurrentDirectory[..^4] + @"\pics\" + imageName);
//            productCurrent.Img = @"\pics\" + imageName;
//        }
//    }
//}





using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;
using System.Globalization;
namespace PL.Product;

enum AddOrUpdate { ADD, UPDATE };

///// <summary>
///// Interaction logic for ProductWindow.xaml
///// </summary>
public partial class ProductWindow : Window
{
    //A private variable to access the logic layer
    BlApi.IBl bl = BlApi.Factory.Get();
// <summary>
// Interaction logic for ProductWindow.xaml
// </summary>

    //dp
    public BO.Product? prod
    {
        get { return (BO.Product?)GetValue(prodProperty); }
        set { SetValue(prodProperty, value); }
    }

    // Using a DependencyProperty as the backing store for prod.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty prodProperty =
        DependencyProperty.Register("prod", typeof(BO.Product), typeof(Window), new PropertyMetadata(null));

    /// <summary>
    /// a flag for the case:
    /// </summary>
    private AddOrUpdate state;
    /// <summary>
    /// ctor for ADD
    /// </summary>
    public ProductWindow()
    {
        InitializeComponent();
        prod = new BO.Product();
        categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Category));    //the combobox contains the categories
        btnOK.Content = "Add Product";  //the button text in ADD case
        state = AddOrUpdate.ADD;    //change the flag
    }

    /// <summary>
    /// ctor for UPDATE
    /// </summary>
    /// <param name="product"> the product to update</param>
    public ProductWindow(int productID)
    {
        InitializeComponent();
        //get a product to update
        try
        {
            prod = bl.product.GetProduct(productID);
        }
        catch { }
        //catch (BO.BlMissingEntityException ex)
        //{
        //    MessageBox.Show("Product Doesn't Exist!!!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        //catch (BO.BlDetailInvalidException ex)
        //{
        //    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        //catch (BO.BlWrongCategoryException ex)
        //{
        //    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        //catch (Exception ex)
        //{
        //    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        //so the details will be written on the window
        categoryComboBox.ItemsSource = Enum.GetValues(typeof(BO.Category));
        iDTextBox.IsReadOnly = true;
        iDTextBox.Foreground = Brushes.DimGray;
        categoryComboBox.SelectedItem = prod?.Category;
        btnOK.Content = "Update";   //the button text in ADD case
        state = AddOrUpdate.UPDATE; //change the flag
    }
    /// <summary>
    /// the button ADD/UPDATE
    /// </summary>
    /// <param name="sender">the sender to the event</param>
    /// <param name="e">the event</param>

    private void TextBox_OnlyNumbers_PreviewKeyDown(object sender, KeyEventArgs e)
    {
        TextBox text = sender as TextBox;
        if (text == null) return;
        if (e == null) return;
        //allow get out of the text box
        if (e.Key == Key.Enter || e.Key == Key.Return || e.Key == Key.Tab)
            return;
        //allow list of system keys (add other key here if you want to allow)
        if (e.Key == Key.Escape || e.Key == Key.Back || e.Key == Key.Delete ||
        e.Key == Key.CapsLock || e.Key == Key.LeftShift || e.Key == Key.Home
        || e.Key == Key.End || e.Key == Key.Insert || e.Key == Key.Down || e.Key == Key.Right || e.Key == Key.NumPad0 || e.Key == Key.NumPad1 || e.Key == Key.NumPad2 || e.Key == Key.NumPad3
        || e.Key == Key.NumPad4 || e.Key == Key.NumPad5 || e.Key == Key.NumPad6 || e.Key == Key.NumPad7 || e.Key == Key.NumPad8 || e.Key == Key.NumPad9)
            return;
        char c = (char)KeyInterop.VirtualKeyFromKey(e.Key);
        //allow control system keys
        if (Char.IsControl(c)) return;
        //allow digits (without Shift or Alt)
        if (Char.IsDigit(c))
            if (!(Keyboard.IsKeyDown(Key.LeftShift) || Keyboard.IsKeyDown(Key.RightAlt)))
                return; //let this key be written inside the textbox
                        //forbid letters and signs (#,$, %, ...)
        e.Handled = true; //ignore this key. mark event as handled, will not be routed to other controls
        return;
    }
    /// <summary>
    /// updates/adds a product
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnOK_Click(object sender, RoutedEventArgs e)
    {
        //for the binding
        iDTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        nameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        priceTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        inStockTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();

        // checking all of the possible problems:
        if (iDTextBox.Text == "" || iDTextBox.Text == "0"
            || categoryComboBox.SelectedItem == null
            || nameTextBox.Text == ""
            || priceTextBox.Text == "" || priceTextBox.Text == "0"
            || inStockTextBox.Text == "" || inStockTextBox.Text == "0")
        {
            //taking care of any problematic case: if there's a problem, color the border in red+show a message 
            if (iDTextBox.Text == "")
            {
                iDTextBox.BorderBrush = Brushes.Red;
                lblErrorID.Content = "❌     ID Field Is Required";
                lblErrorID.Visibility = Visibility.Visible;
            }
            else if (iDTextBox.Text == "0")
            {
                iDTextBox.BorderBrush = Brushes.Red;
                lblErrorID.Content = "❌     ID Field Is Invalid";
                lblErrorID.Visibility = Visibility.Visible;
            }

            if (categoryComboBox.SelectedItem == null)
            {
                categoryComboBox.BorderBrush = Brushes.Red;
                lblErrorCategory.Visibility = Visibility.Visible;
            }
            if (nameTextBox.Text == "")
            {
                nameTextBox.BorderBrush = Brushes.Red;
                lblErrorName.Visibility = Visibility.Visible;
            }
            if (priceTextBox.Text == "")
            {
                priceTextBox.BorderBrush = Brushes.Red;
                lblErrorPrice.Content = "❌     Price Field Is Required";
                lblErrorPrice.Visibility = Visibility.Visible;
            }
            else if (priceTextBox.Text == "0")
            {
                priceTextBox.BorderBrush = Brushes.Red;
                lblErrorPrice.Content = "❌     Price Field Is Invalid";
                lblErrorPrice.Visibility = Visibility.Visible;
            }
            if (inStockTextBox.Text == "")
            {
                inStockTextBox.BorderBrush = Brushes.Red;
                lblErrorInStock.Content = "❌     In Stock Field Is Required";
                lblErrorInStock.Visibility = Visibility.Visible;
            }
            else if (inStockTextBox.Text == "0")
            {
                inStockTextBox.BorderBrush = Brushes.Red;
                lblErrorInStock.Content = "❌     In Stock Field Is Invalid";
                lblErrorInStock.Visibility = Visibility.Visible;
            }
            // let the user change his mistake:
            return;
        }


        //ADD:
        if (state == AddOrUpdate.ADD)
        {
            try
            {
                treatImage();//the fuction treat all things relevant to the image
                bl.product.AddProduct(prod);
                MessageBox.Show("Product Added Successfuly😊", "💍", MessageBoxButton.OK);
                this.Close();
            }
            catch { }
            //catch (BO.BlAlreadyExistsEntityException ex)
            //{
            //    MessageBox.Show("Product Already Exists!!!", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            //catch (BO.BlWrongCategoryException ex)
            //{
            //    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
        }
        //UPDATE:
        else
        {
            try
            {
                treatImage();//the fuction treat all things relevant to the image
                bl.product.UpDateProduct(prod);
                MessageBox.Show("Product Updated Successfuly😊", "💍", MessageBoxButton.OK);
                this.Close();
            }
            catch { }
            //catch (BO.BlWrongCategoryException ex)
            //{
            //    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}
        }
    }

    /// <summary>
    /// if there was a mistake, when the user is typing again remove the messages and red color
    /// </summary>
    /// <param name = "sender" > the sender to the event</param>
    /// <param name="e">the event</param>
    private void iDTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        iDTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();

        if (iDTextBox.BorderBrush == Brushes.Red)
        {
            iDTextBox.BorderBrush = Brushes.DimGray;
            lblErrorID.Visibility = Visibility.Hidden;
        }
    }
    /// <summary>
    /// if there was a mistake, when the user is typing again remove the messages and red color
    /// </summary>
    /// <param name = "sender" > the sender to the event</param>
    /// <param name="e">the event</param>
    private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

        if (categoryComboBox.BorderBrush == Brushes.Red)
        {
            categoryComboBox.BorderBrush = Brushes.DimGray;
            lblErrorCategory.Visibility = Visibility.Hidden;
        }
    }
    /// <summary>
    /// if there was a mistake, when the user is typing again remove the messages and red color
    /// </summary>
    /// <param name="sender">the sender to the event</param>
    /// <param name="e">the event</param>
    private void nameTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        nameTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();

        if (nameTextBox.BorderBrush == Brushes.Red)
        {
            nameTextBox.BorderBrush = Brushes.DimGray;
            lblErrorName.Visibility = Visibility.Hidden;
        }
    }
    /// <summary>
    /// if there was a mistake, when the user is typing again remove the messages and red color
    /// </summary>
    /// <param name="sender">the sender to the event</param>
    /// <param name="e">the event</param>
    private void categoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (categoryComboBox.BorderBrush == Brushes.Red)
        {
            categoryComboBox.BorderBrush = Brushes.DimGray;
            lblErrorCategory.Visibility = Visibility.Hidden;
        }

    }
    /// <summary>
    /// if there was a mistake, when the user is typing again remove the messages and red color
    /// </summary>
    /// <param name="sender">the sender to the event</param>
    /// <param name="e">the event</param>
    private void priceTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        priceTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        if (priceTextBox.BorderBrush == Brushes.Red)
        {
            priceTextBox.BorderBrush = Brushes.DimGray;
            lblErrorPrice.Visibility = Visibility.Hidden;
        }
    }
    /// <summary>
    /// if there was a mistake, when the user is typing again remove the messages and red color
    /// </summary>
    /// <param name="sender">the sender to the event</param>
    /// <param name="e">the event</param>
    private void inStockTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        inStockTextBox.GetBindingExpression(TextBox.TextProperty).UpdateSource();
        if (inStockTextBox.BorderBrush == Brushes.Red)
        {
            inStockTextBox.BorderBrush = Brushes.DimGray;
            lblErrorInStock.Visibility = Visibility.Hidden;
        }
    }


    /// <summary>
    /// let the manager select an image from the browser
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        OpenFileDialog openFileDialog = new OpenFileDialog();
        if (openFileDialog.ShowDialog() == true)
        {
            pbx.Source = new BitmapImage(new Uri(openFileDialog.FileName));
            prod.Img = openFileDialog.FileName;

        }
    }

    /// <summary>
    /// close the current window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Button_Click(object sender, RoutedEventArgs e) => this.Close();

    /// <summary>
    /// the function treats the things of the image
    /// </summary>
    private void treatImage()
    {
        if (prod.Img != null)
        {
            string imageName = prod.Img.Substring(prod.Img.LastIndexOf("\\"));
            if (!File.Exists(Environment.CurrentDirectory[..^4] + @"/PL/img/catalog/" + imageName))
                File.Copy(prod.Img, Environment.CurrentDirectory[..^4] + @"/PL/img/catalog/" + imageName);
            prod.Img = @"/PL/img/catalog/" + imageName;
        }
    }
}


class ConvertPathToBitmapImag : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        try
        {
            //if (value == null)
            //{
            //    return null;
            //}
            string imageRelativeName = (string)value;
            string currentDir = Environment.CurrentDirectory[..^4];
            string imageFullName = currentDir + imageRelativeName;//direction of the picture
            BitmapImage bitmapImage = new BitmapImage(new Uri(imageRelativeName, UriKind.Relative));//makes the picture
            return bitmapImage;

        }
        catch
        {
            string imageRelativeName = @"\pics\default.jpg";//default picture
            string currentDir = Environment.CurrentDirectory[..^4];
            string imageFullName = currentDir + imageRelativeName;//direction of the picture
            BitmapImage bitmapImage = new BitmapImage(new Uri(imageFullName));//makes the picture
            return bitmapImage;
        }
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}


