using System.Windows;
namespace PL.Cart;

/// <summary>
/// Interaction logic for paymentWindow.xaml
/// </summary>
public partial class paymentWindow : Window
{

    public BO.Cart MyCart
    {
        get { return (BO.Cart)GetValue(MyCartProperty); }
        set { SetValue(MyCartProperty, value); }
    }

    // Using a DependencyProperty as the backing store for MyCart.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty MyCartProperty =
        DependencyProperty.Register("MyCart", typeof(BO.Cart), typeof(paymentWindow), new PropertyMetadata(null));

    public paymentWindow(BO.Cart getCart)
    {
        InitializeComponent();
        MyCart=getCart;
    }

    private void Button_Click(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}
