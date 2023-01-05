using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace PL.Order
{
    /// <summary>
    /// Interaction logic for OrderTrackinkWindow.xaml
    /// </summary>
    public partial class OrderTrackinkWindow : Window
    {
        BlApi.IBl bl = BlApi.Factory.Get();

        BO.Order? order;
        //public BO.Order? order
        //{
        //    get { return (BO.Order?)GetValue(orderProperty); }
        //    set { SetValue(orderProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for order.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty orderProperty =
        //    DependencyProperty.Register("order", typeof(BO.Order), typeof(OrderTrackinkWindow), new PropertyMetadata(0));

        BO.OrderTracking? tracking;
        //public BO.OrderTracking? tracking
        //{
        //    get { return (BO.OrderTracking?)GetValue(trackingProperty); }
        //    set { SetValue(trackingProperty, value); }
        //}

        //// Using a DependencyProperty as the backing store for tracking.  This enables animation, styling, binding, etc...
        //public static readonly DependencyProperty trackingProperty =
        //    DependencyProperty.Register("tracking", typeof(BO.OrderTracking), typeof(OrderTrackinkWindow), new PropertyMetadata(0));



        //public OrderTrackinkWindow()
        //{
        //    InitializeComponent();
        //}
        public OrderTrackinkWindow(int orderId)
        {
            InitializeComponent();

            order = bl.order.GetOrderDetails(orderId);
            tracking = bl.order.OrderTracking(order.ID);
            lsvTracking.ItemsSource = tracking.Tracking;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new OrderWindow(order.ID).Show();
        }
    }
}
