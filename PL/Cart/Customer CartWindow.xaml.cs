using System.Windows;
namespace PL.Cart;





/// <summary>
/// Interaction logic for Customer_CartWindow.xaml
/// </summary>
public partial class Customer_CartWindow : Window
{
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
        InitializeComponent();
        MyCart = getCart;

    }
}
