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
using BO;
using Path = System.IO.Path;

namespace PL.Product;

enum AddOrUpdate { ADD, UPDATE };

///// <summary>
///// Interaction logic for ProductWindow.xaml
///// </summary>
public partial class ProductWindow : Window
{
    //A private variable to access the logic layer
    BlApi.IBl bl = BlApi.Factory.Get();

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
        
            
        catch (BO.InternalErrorException ex)
        {
            MessageBox.Show("Product Doesn't Exist!!!", "OK?", MessageBoxButton.OK);
        }
        catch (BO.InvalidArgumentException ex)
        {
            MessageBox.Show(ex.ToString(), "OK?", MessageBoxButton.OK);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }
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
        prod.Category = (BO.Category)categoryComboBox.SelectedItem;
        //prod.Img= pbx.Source.ToString();
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
                MessageBox.Show("Product Added Successfuly😊", "🍰", MessageBoxButton.OK);
                this.Close();
            }
            catch (BO.InvalidArgumentException ex)
            {
                MessageBox.Show("Product Already Exists!!!", "OK?", MessageBoxButton.OK);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
        //UPDATE:
        else
        {
            try
            {
                treatImage();//the fuction treat all things relevant to the image
                bl.product.UpDateProduct(prod);
                MessageBox.Show("Product Updated Successfuly😊", "🍰", MessageBoxButton.OK);
                this.Close();
            }
            catch (BO.InternalErrorException ex)
            {
                MessageBox.Show(ex.ToString(), "OK?", MessageBoxButton.OK);
                return;
            }
            catch (BO.InvalidArgumentException ex)
            {
                MessageBox.Show(ex.ToString(), "OK?", MessageBoxButton.OK);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
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

            //string imageName = prod.Img.Substring(prod.Img.LastIndexOf("/"));
            //if (!File.Exists(Environment.CurrentDirectory[..^4] + @"/PL/img/catalog" + imageName))
            //    File.Copy(prod.Img, Environment.CurrentDirectory[..^4] + @"/PL/img/catalog" + imageName);
            //prod.Img = @"/img/catalog" + imageName;
            string imageName = Path.GetFileName(prod.Img);
           
            if (!File.Exists(Environment.CurrentDirectory[..^4] + @"/PL/img/catalog/" + imageName))
                File.Copy(prod.Img, Environment.CurrentDirectory[..^4] + @"/PL/img/catalog/" + imageName);
            prod.Img = @"/img/catalog/" + imageName;
          
        }
    }
}

class ConvertPathToBitmapImag : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {

        try
        {
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


