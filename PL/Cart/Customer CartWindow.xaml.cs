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
    List<Tuple<int, int>> items = new List<Tuple<int, int>>();

    private BlApi.IBl bl = BlApi.Factory.Get();

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
            MyCart = bl.cart.Update(MyCart, item.Item1, item.Item2);
        }
    }

    private void mytxt_LostFocus(object sender, RoutedEventArgs e)
    {
        int amount = int.Parse(((TextBox)sender).Text);
        var oldAmount = ((BO.OrderItem)((TextBox)sender).DataContext).QuantityPerItem;
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
        MyCart = bl.cart.Delete(MyCart, selection.ProductId);
    }

    private void btnPayment_Click(object sender, RoutedEventArgs e)
    {
        new paymentWindow(MyCart).ShowDialog();
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }



    //private void Button_Click(object sender, RoutedEventArgs e)
    //{
    //   BO.OrderItem selection = ((BO.OrderItem)((TextBox)sender).DataContext);
    //    MyCart = bl.cart

    //}
}


public class NotBooleanToVisibilityConverter : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Cart cart = (BO.Cart)value;
        if (cart!=null&&cart.items != null && cart.items.Count != 0)
        {
            return Visibility.Hidden; //Visibility.Collapsed;
        }
        else
        {
            return Visibility.Visible;
        }
    }

    //convert from target property type to source property type
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }

   // private void btnPayment_Click(object sender, RoutedEventArgs e) => new paymentWindow(MyCart).ShowDialog();
}

public class NotBooleanToVisibilityConverter2 : IValueConverter
{
    //convert from source property type to target property type
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        BO.Cart cart = (BO.Cart)value;
        if (cart != null && cart.items != null && cart.items.Count != 0)
        {
            return Visibility.Visible; //Visibility.Collapsed;
        }
        else
        {
            return Visibility.Hidden;
        }
    }

    //convert from target property type to source property type
    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}