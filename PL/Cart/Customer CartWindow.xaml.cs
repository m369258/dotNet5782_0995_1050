using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace PL.Cart;

/// <summary>
/// Interaction logic for Customer_CartWindow.xaml
/// </summary>
public partial class Customer_CartWindow : Window
{
    private BlApi.IBl bl = BlApi.Factory.Get();

    List<Tuple<int, int>> items = new List<Tuple<int, int>>();

    public bool state
    {
        get { return (bool)GetValue(stateProperty); }
        set { SetValue(stateProperty, value); }
    }
    // Using a DependencyProperty as the backing store for state.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty stateProperty =
        DependencyProperty.Register("state", typeof(bool), typeof(Customer_CartWindow), new PropertyMetadata(false));

    public BO.Cart MyCart
    {
        get { return (BO.Cart)GetValue(MyCartProperty); }
        set { SetValue(MyCartProperty, value); }
    }
    // Using a DependencyProperty as the backing store for MyCart.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyCartProperty =
        DependencyProperty.Register("MyCart", typeof(BO.Cart), typeof(Customer_CartWindow), new PropertyMetadata(null));

    public Customer_CartWindow(BO.Cart getCart)
    {
        MyCart = new BO.Cart();
        InitializeComponent();
        MyCart = getCart;
    }

    private void btnUpdateCart_Click(object sender, RoutedEventArgs e)
    {
        foreach (var item in items)
        {
            try
            {
                BO.Cart tmp2Cart = bl.cart.Update(MyCart, item.Item1, item.Item2);
                MyCart = new BO.Cart();
                MyCart = tmp2Cart;
            }
            catch (BO.InvalidInputException ex) { MessageBox.Show("Pay attention" + ex.Message); }
            catch (BO.InternalErrorException ex) { MessageBox.Show("Pay attention" + ex.Message); }
            catch (BO.NotEnoughInStockException) { MessageBox.Show("We are sorry but the item is out of stock"); }
            catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error); return; }
        }
    }

    private void mytxt_LostFocus(object sender, RoutedEventArgs e)
    {
        int amount = int.Parse(((TextBox)sender).Text);
        int oldAmount = ((BO.OrderItem)((TextBox)sender).DataContext).QuantityPerItem;
        int AmountInStockOfProduct = bl.product.GetProduct(((BO.OrderItem)((TextBox)sender).DataContext).ProductId).InStock;

        if (amount > AmountInStockOfProduct)
        {
            MessageBox.Show("We are sorry but the item is out of stock");
            ((TextBox)sender).Text = oldAmount.ToString();
            return;
        }
        //only if was change
        if (amount != oldAmount)
        {
            state = true;
            BO.OrderItem selection = ((BO.OrderItem)((TextBox)sender).DataContext);
            items?.Add(new Tuple<int, int>(selection.ProductId, amount));
        }

    }

    private void btnDeleteItemFromCart_Click(object sender, RoutedEventArgs e)
    {
        BO.OrderItem selection = ((BO.OrderItem)((Button)sender).DataContext);

        try { MyCart = bl.cart.Delete(MyCart, selection.ProductId); }
        catch (BO.InternalErrorException) { MessageBox.Show("We are sorry but the item is not exsist"); }
        catch (Exception ex) { MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error); return; }
    }

    private void btnPayment_Click(object sender, RoutedEventArgs e)
    {
        new paymentWindow(MyCart).ShowDialog();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
