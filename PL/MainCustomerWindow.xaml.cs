﻿using PL.Cart;
using PL.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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

    private int numCategory;

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


    private string? clickOn = "";

    /// <summary>
    /// ctor - Reboots the page for the catalog of all products.
    /// </summary>
    /// <param name="cart"></param>
    public MainCustomerWindow(BO.Cart cart = null)
    {
        InitializeComponent();
        if (cart != null)
            MyCart = cart;
        else
            MyCart = new BO.Cart();

        try
        {
            IEnumerable<BO.ProductItem> temp = bl.product.GetCatalog(0, MyCart.items);
            MyProductItems = temp == null ? new() : new(temp);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// Filtering the catalog by desired category.
    /// </summary>
    /// <param name="sender">The textblock on which the catalog is filtered</param>
    private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        BO.Category c;
        string? strCat = (sender as TextBlock)?.Text;
        try
        {
            clickOn = strCat;
            BO.Category.TryParse(strCat, out c);
            numCategory = (int)c;

            if (strCat != "popular")
            {
                var temp = bl.product.GetCatalog(numCategory, MyCart.items);
                MyProductItems = temp == null ? new() : new(temp);
            }
            else
            {
                var temp = bl.product.PopularItems(MyCart.items);
                MyProductItems = temp == null ? new() : new(temp);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    /// <summary>
    /// log out from the system
    /// </summary>
    private void menuLogOut_Click(object sender, RoutedEventArgs e) { MyCart = null; new MainWindow().Show(); this.Close(); }

    /// <summary>
    /// Adding a quantity to an item.
    /// </summary>
    /// <param name="sender">The visual button to increase the quantity in 1.</param>
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

                if (clickOn != "popular")
                {
                    var temp = bl.product.GetCatalog(numCategory, MyCart.items);
                    MyProductItems = temp == null ? new() : new(temp);
                }
                else
                {
                    var temp = bl.product.PopularItems(MyCart.items);
                    MyProductItems = temp == null ? new() : new(temp);
                }
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

    /// <summary>
    /// Quantity reduction per item.
    /// </summary>
    /// <param name="sender">The visual button to decrease the quantity by 1.</param>
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

        if (clickOn != "popular")
        {
            var temp = bl.product.GetCatalog(numCategory, MyCart.items);
            MyProductItems = temp == null ? new() : new(temp);
        }
        else
        {
            var temp = bl.product.PopularItems(MyCart.items);
            MyProductItems = temp == null ? new() : new(temp);
        }

    }

    /// <summary>
    /// Opening the basket only if there are products.
    /// </summary>
    private void Button_Click(object sender, RoutedEventArgs e)
    {
        if (MyCart.items != null)
        {
            Customer_CartWindow customer_CartWindow = new Customer_CartWindow(MyCart);
            customer_CartWindow.Show();

            MyCart = customer_CartWindow.MyCart;

            var temp = bl.product.GetCatalog(0, MyCart.items);
            MyProductItems = temp == null ? new() : new(temp);
            this.Close();
        }
    }

    /// <summary>
    /// Opening the tracking window and closing the current window.
    /// </summary>
    private void menuTracking_Click(object sender, RoutedEventArgs e)
    {
        new OrderTrackinkWindow(MyCart).Show();
        this.Close();
    }

    /// <summary>
    /// Opening the login window and closing the current window.
    /// </summary>
    private void menuSignIn_Click(object sender, RoutedEventArgs e)
    {
        new LogInWindow().Show();
        this.Close();
    }

    /// <summary>
    /// Back to the appropriate page
    /// </summary>
    private void txtBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        new MainWindow().Show();
        this.Close();
    }
}
