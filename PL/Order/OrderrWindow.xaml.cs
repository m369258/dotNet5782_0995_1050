//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Data;
//using System.Windows.Documents;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Imaging;
//using System.Windows.Shapes;

//namespace PL.Order;

///// <summary>
///// Interaction logic for OrderrWindow.xaml
///// </summary>
//public partial class OrderrWindow : Window
//{
//    BlApi.IBl bl = BlApi.Factory.Get();
//    public BO.Order myOrder
//    {
//        get { return (BO.Order)GetValue(myOrderProperty); }
//        set { SetValue(myOrderProperty, value); }
//    }

//    // Using a DependencyProperty as the backing store for myOrder.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty myOrderProperty =
//        DependencyProperty.Register("myOrder", typeof(BO.Order), typeof(OrderWindow), new PropertyMetadata(null));

//    public int ID
//    {
//        get { return (int)GetValue(IDProperty); }
//        set { SetValue(IDProperty, value); }
//    }

//    // Using a DependencyProperty as the backing store for ID.  This enables animation, styling, binding, etc...
//    public static readonly DependencyProperty IDProperty =
//        DependencyProperty.Register("ID", typeof(int), typeof(OrderWindow), new PropertyMetadata(0));


//    public OrderrWindow(int id)
//    {
//        myOrder = new BO.Order();
//        InitializeComponent();
//        ID = id;
//        try { myOrder = bl.order.GetOrderDetails(id); }
//        catch { MessageBox.Show("בעיה בהזמנה"); }
//    }

//    private void btnUpdate_Click(object sender, RoutedEventArgs e)
//    {
//        if (myOrder.status == (BO.OrderStatus)2)
//        {
//            try { bl.order.OrderDeliveryUpdate(myOrder.ID); }
//            catch (BO.InternalErrorException ex) { MessageBox.Show(ex.Message); }
//        }
//        else
//        {
//            try { bl.order.OrderShippingUpdate(myOrder.ID); }
//            catch { MessageBox.Show("יש בעיה בהזמנה"); }
//        }
//    }
//}


/////// <summary>
/////// converts visibility of update delivery button according to if it's manager and if was shipping and delivery already
/////// </summary>
////public class SelectedDateToVisibilityConverter1 : IMultiValueConverter
////{
////    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
////    {
////        if ((DateTime?)values[0] == null && (string)values[1] == null)//was'nt shipping and it's manager
////        {
////            return Visibility.Visible;
////        }
////        else
////        {
////            return Visibility.Collapsed;
////        }
////    }

////    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
////    {
////        throw new NotImplementedException();
////    }
////}

/////// <summary>
/////// converts visibility of update delivery button according to if it's manager and if was shipping already
/////// </summary>
////public class Selected2DatesToVisibilityConverter1 : IMultiValueConverter
////{
////    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
////    {
////        if ((DateTime?)values[0] != null && (DateTime?)values[1] == null && (string)values[2] == null)//was'nt delivery and was shipping and it's manager
////        {
////            return Visibility.Visible;
////        }
////        else
////        {
////            return Visibility.Collapsed;
////        }
////    }

////    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
////    {
////        throw new NotImplementedException();
////    }
////}

