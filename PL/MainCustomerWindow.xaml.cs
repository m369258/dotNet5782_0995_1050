using BO;
using PL.Cart;
using PL.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
namespace PL;

/// <summary>
/// Interaction logic for CustomerWindow.xaml
/// </summary>
public partial class MainCustomerWindow : Window
{
    /// <summary>
    /// Bl object to have an access to the Bl functions
    /// </summary>
    private BlApi.IBl bl = BlApi.Factory.Get();

    public ObservableCollection<BO.ProductItem> MyProductItems
    {
        get { return (ObservableCollection<BO.ProductItem>)GetValue(MyProductItemsProperty); }
        set { SetValue(MyProductItemsProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyProductItems.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyProductItemsProperty =
        DependencyProperty.Register("MyProductItems", typeof(ObservableCollection<BO.ProductItem>), typeof(MainCustomerWindow), new PropertyMetadata(null));

    public BO.Cart MyCart
    {
        get { return (BO.Cart)GetValue(MyCartProperty); }
        set { SetValue(MyCartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyCart.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyCartProperty =
    DependencyProperty.Register("MyCart", typeof(BO.Cart), typeof(MainCustomerWindow), new PropertyMetadata(null));


    //BO.Cart MyCart = new BO.Cart();


    /// <summary>
    /// ctor for customer window
    /// </summary>
    /// <param name="cart">the cart of the user</param>
    public MainCustomerWindow()
    {
        MyCart = new BO.Cart();
        //curOrderItem=new BO.OrderItem();
        InitializeComponent();
        ////try
        ////{
        //catalog.ItemsSource = bl.product.GetListOfProducts();
        IEnumerable<BO.ProductItem> temp = bl.product.GetCatalog();
        //var temp = bl.product.GetCatalog();
        MyProductItems = temp == null ? new() : new(temp);
        ////}
        //catch (BO.BlNullPropertyException ex)
        //{
        //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        //catch (BO.BlWrongCategoryException ex)
        //{
        //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        ////catch (Exception ex)
        ////{
        ////    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        ////}
        // prod = new ObservableCollection<BO.ProductForList>(bl.Product.ListOfProducts().OrderBy(x => x?.ID));
        // this.DataContext = prod;
    }

    public MainCustomerWindow(BO.Cart cart)
    {
        MyCart = new BO.Cart();
        MyCart = cart;
        //curOrderItem=new BO.OrderItem();
        InitializeComponent();
        try
        {
            //catalog.ItemsSource = bl.product.GetListOfProducts();
            IEnumerable<BO.ProductItem> temp = bl.product.GetCatalog();
            //var temp = bl.product.GetCatalog();
            MyProductItems = temp == null ? new() : new(temp);
        }
        //catch (BO.BlNullPropertyException ex)
        //{
        //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        //catch (BO.BlWrongCategoryException ex)
        //{
        //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        // prod = new ObservableCollection<BO.ProductForList>(bl.Product.ListOfProducts().OrderBy(x => x?.ID));
        // this.DataContext = prod;
    }
    /// <summary>
    /// select an item and show its details
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// click על + ועל - ויעשה updatecart
    //private void ListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
    //{
    //    BO.ProductItem? selectionProductItem = (BO.ProductItem?)(catalog.SelectedItem);
    //    try
    //    {

    //        //open the window to update the product which was selected
    //        //ProductWindow prodWin = new ProductWindow(prod?.ID ?? throw new NullReferenceException("You have to choose a product to update!"), cart);
    //        //prodWin.ShowDialog();
    //    }
    //    //catches for the exception which mighgt be thrown from the ProductDetailsForManager function
    //    catch (NullReferenceException ex)
    //    {
    //        MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
    //    }
    //    catch (Exception ex)
    //    {
    //        MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
    //    }
    //}
    /// <summary>
    /// button for opening cart window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //private void Button_Click(object sender, RoutedEventArgs e) => new CartWindow(cart).ShowDialog();
    /// <summary>
    /// select the catgory to show
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        BO.Category c;
        string? strCat = (sender as TextBlock)?.Text;
        try
        {
            if (strCat != "Popular")
            {
                BO.Category.TryParse(strCat, out c);

                var temp = bl.product.GetCatalog((int)c, MyCart.items);
                MyProductItems = temp == null ? new() : new(temp);


                //  catalog.ItemsSource = bl.product.GetListOfProducts((int)c);
            }
            else
            {
                var temp = bl.product.GetCatalog(0, MyCart.items);
                MyProductItems = temp == null ? new() : new(temp);
                //catalog.ItemsSource = bl.product.GetListOfProducts();

            }
        }
        //catch (BO.BlNullPropertyException ex)
        //{
        //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        //catch (BO.BlWrongCategoryException ex)
        //{
        //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    /// <summary>
    /// reset for the page
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        //// try
        ////  {
        var temp = bl.product.GetCatalog();
        MyProductItems = temp == null ? new() : new(temp);
        // catalog.ItemsSource = bl.product.GetListOfProducts();
        ////  }
        //catch (BO.BlNullPropertyException ex)
        //{
        //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        //catch (BO.BlWrongCategoryException ex)
        //{
        //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
        ////catch (Exception ex)
        ////{
        ////    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        ////}
    }
    /// <summary>
    /// open my orders window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>

    // private void menuOrders_Click(object sender, RoutedEventArgs e) => new OrderWindow(cart.CustomerEmail).ShowDialog();

    //private void menuTracking_Click(object sender, RoutedEventArgs e) => new OrderTrackinkWindow().ShowDialog();


    private void menuLogOut_Click(object sender, RoutedEventArgs e) => this.Close();

    private void btnPlus_Click(object sender, RoutedEventArgs e)
    {
        BO.ProductItem selectionProductItem = ((BO.ProductItem)((Button)sender).DataContext);
        try
        {
            if (selectionProductItem.InStock)
            {
                if (selectionProductItem.Amount == 0)
                {
                    MyCart = bl.cart.Add(MyCart, selectionProductItem.ProductID);
                }
                else
                {
                    MyCart = bl.cart.Update(MyCart, selectionProductItem.ProductID, selectionProductItem.Amount + 1);
                }

                var temp = bl.product.GetCatalog(0, MyCart.items);
                MyProductItems = temp == null ? new() : new(temp);
            }
            else
                MessageBox.Show("Product out of stock");
        }

        catch (BO.InvalidInputException ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        catch (BO.InternalErrorException ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        catch (BO.NotEnoughInStockException ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            return;
        }
    }

    private void btnMinus_Click(object sender, RoutedEventArgs e)
    {
        BO.ProductItem selectionProductItem = ((BO.ProductItem)((Button)sender).DataContext);


        if (selectionProductItem.Amount != 0)
        {
            try { MyCart = bl.cart.Update(MyCart, selectionProductItem.ProductID, selectionProductItem.Amount - 1); }
            catch (BO.InvalidInputException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.InternalErrorException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (BO.NotEnoughInStockException ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        var temp = bl.product.GetCatalog(0, MyCart.items);
        MyProductItems = temp == null ? new() : new(temp);

    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (MyCart.items != null)
        {
            Customer_CartWindow customer_CartWindow = new Customer_CartWindow(MyCart);
            customer_CartWindow.ShowDialog();

            MyCart = customer_CartWindow.MyCart;

            var temp = bl.product.GetCatalog(0, MyCart.items);
            MyProductItems = temp == null ? new() : new(temp);
        }
    }

    private void menuTracking_Click(object sender, RoutedEventArgs e)
    {
        new OrderTrackinkWindow().ShowDialog();
    }
}
