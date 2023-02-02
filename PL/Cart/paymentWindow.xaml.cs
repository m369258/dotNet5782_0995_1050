using System.Windows;
namespace PL.Cart;

/// <summary>
/// Interaction logic for paymentWindow.xaml
/// </summary>
public partial class paymentWindow : Window
{
    private BlApi.IBl bl = BlApi.Factory.Get();

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

    private void Button_Click_1(object sender, RoutedEventArgs e)
    {
        try
        {
            bl.cart.MakeAnOrder(MyCart);
        }
        catch (BO.InvalidArgumentException ex) { MessageBox.Show("Please change the" + ex.Message); }
        catch(BO.InternalErrorException ex) { MessageBox.Show(ex.Message); }
        MessageBox.Show("theOrder made");
    }

}
