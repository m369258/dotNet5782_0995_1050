using PL.Cart;
using PL.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
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


    public bool IsInStock
    {
        get { return (bool)GetValue(IsInStockProperty); }
        set { SetValue(IsInStockProperty, value); }
    }

    // Using a DependencyProperty as the backing store for IsInStock.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty IsInStockProperty =
        DependencyProperty.Register("IsInStock", typeof(bool), typeof(MainCustomerWindow), new PropertyMetadata(true));



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

    public MainCustomerWindow(BO.Cart cart = null)
    {
        InitializeComponent();
        if (cart != null)
            MyCart = cart;
        else
            MyCart = new BO.Cart();

        try
        {
            IEnumerable<BO.ProductItem> temp = bl.product.GetCatalog();
            MyProductItems = temp == null ? new() : new(temp);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
    
    private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        BO.Category c;
        string? strCat = (sender as TextBlock)?.Text;
        try
        {
            if (strCat != "popular")
            {
                BO.Category.TryParse(strCat, out c);

                var temp = bl.product.GetCatalog((int)c, MyCart.items);
                MyProductItems = temp == null ? new() : new(temp);
            }
            else
            {
                var temp = bl.product.PopularItems();
                MyProductItems = temp == null ? new() : new(temp);
            }
        }
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
        var temp = bl.product.GetCatalog();
        MyProductItems = temp == null ? new() : new(temp);
    }
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
            {
                IsInStock = false;
                MessageBox.Show("Product out of stock");
                return;
            }
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
        catch (BO.NotEnoughInStockException)
        {
            IsInStock = false;

            System.Windows.MessageBox.Show("This product is out of stock");
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
                System.Windows.MessageBox.Show(ex.Message, "Problrm:(", MessageBoxButton.OK, MessageBoxImage.Error);
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
        new OrderTrackinkWindow(MyCart.CustomerEmail).Show();
        this.Close();
    }

    private void menuSignIn_Click(object sender, RoutedEventArgs e)
    {
        new LogInWindow().Show();
        this.Close();
    }
}




