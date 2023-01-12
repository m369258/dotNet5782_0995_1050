using BO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace PL.Cart;

/// <summary>
/// Interaction logic for Customer_CartWindow.xaml
/// </summary>
public partial class Customer_CartWindow : Window
{
    List<Tuple<int, int>> items=new List<Tuple<int, int>>();

    private BlApi.IBl bl = BlApi.Factory.Get();

    public BO.Cart MyCart
    {
        get { return (BO.Cart)GetValue(MyCartProperty); }
        set { SetValue(MyCartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyCart.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyCartProperty =
        DependencyProperty.Register("MyCart", typeof(BO.Cart), typeof(Customer_CartWindow), new PropertyMetadata(null));

    public Customer_CartWindow(ref BO.Cart getCart)
    {
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
        int amount = int .Parse(((TextBox)sender).Text);
        BO.OrderItem selection = ((BO.OrderItem)((TextBox)sender).DataContext);
        items?.Add(new Tuple<int, int>(selection.ProductId, amount));
    }
}
