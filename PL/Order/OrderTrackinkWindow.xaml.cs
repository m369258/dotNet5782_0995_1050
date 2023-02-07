using PL.Order;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace PL.Order;

/// <summary>
/// Interaction logic for OrderTrackinkWindow.xaml
/// </summary>
public partial class OrderTrackinkWindow : Window
{
    BlApi.IBl bl = BlApi.Factory.Get();
    string email;

   


    public ObservableCollection<BO.OrderTracking?> OrdersID
    {
        get { return (ObservableCollection<BO.OrderTracking?>)GetValue(OrdersIDProperty); }
        set { SetValue(OrdersIDProperty, value); }
    }

    // Using a DependencyProperty as the backing store for OrdersID.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty OrdersIDProperty =
        DependencyProperty.Register("OrdersID", typeof(ObservableCollection<BO.OrderTracking?>), typeof(OrderTrackinkWindow), new PropertyMetadata(null));




    BO.Order? order;
    //dp
    public BO.OrderTracking? tracking
    {
        get { return (BO.OrderTracking?)GetValue(trackingProperty); }
        set { SetValue(trackingProperty, value); }
    }

    // Using a DependencyProperty as the backing store for tracking.  This enables animation, styling, binding, etc...
    public static readonly DependencyProperty trackingProperty =
        DependencyProperty.Register("tracking", typeof(BO.OrderTracking), typeof(OrderTrackinkWindow), new PropertyMetadata(null));



    /// <summary>
    /// ctor for order tracking window
    /// </summary>
    public OrderTrackinkWindow(string email=null)
    {
        InitializeComponent();
        this.email = email;
        try
        {
            if (email != "")   //show user's orders
            {
                var temp = bl.order.OrdersOfUsers(email);
                OrdersID = temp == null ? new() : new(temp);

            }
            else    //show all orders (for manager)
            {
                var temp = bl.order.OrdersOfUsers();
                OrdersID = temp == null ? new() : new(temp);
            }
        }
        catch { }
        //catch (BO.BlNullPropertyException ex)
        //{
        //    MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        //}
    }
    /// <summary>
    /// button for getting the order to track by id
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void btnOrderDetails_Click(object sender, RoutedEventArgs e)
    {
        //if (cmbOrders.SelectedItem as BO.OrderForList != null)
        //{
       //  new OrderWindow(order.ID).ShowDialog();
            //try
            //{
              //  if (mail != null)   //show user's orders
                //{
                //    //cmbOrders.ItemsSource = bl.order.GetListOfOrders(mail);
                //    cmbOrders.ItemsSource = bl.order.GetListOfOrders();

                //}
                //else    //show all orders (for manager)
                    //cmbOrders.ItemsSource = bl.order.GetListOfOrders();
            //}
            //catch (BO.BlNullPropertyException ex)
            //{
            //    MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
           // lstTracking.ItemsSource = null;
        //}
    }

    
    
    /// <summary>
    /// close window
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void txtBack_MouseLeftButtonDown(object sender, MouseButtonEventArgs e) => this.Close();

    /// <summary>
    /// show tracking of the selected order in the combobox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        BO.OrderTracking    selectedOrder = ((BO.OrderTracking)((ComboBox)sender).SelectedItem);
        tracking = bl.order.OrderTracking(selectedOrder.ID);

    }






    //private static void TargetComboBox_Loaded(object sender, RoutedEventArgs e)
    //{
    //    var targetComboBox = sender as ComboBox;
    //    var targetTextBox = targetComboBox?.Template.FindName("PART_EditableTextBox", targetComboBox) as TextBox;

    //    if (targetTextBox == null) return;

    //    targetComboBox.Tag = "TextInput";
    //    targetComboBox.StaysOpenOnEdit = true;
    //    targetComboBox.IsEditable = true;
    //    targetComboBox.IsTextSearchEnabled = false;

    //    targetTextBox.TextChanged += (o, args) =>
    //    {
    //        var textBox = (TextBox)o;

    //        var searchText = textBox.Text;

    //        if (targetComboBox.Tag.ToString() == "Selection")
    //        {
    //            targetComboBox.Tag = "TextInput";
    //            targetComboBox.IsDropDownOpen = true;
    //        }
    //        else
    //        {
    //            if (targetComboBox.SelectionBoxItem != null)
    //            {
    //                targetComboBox.SelectedItem = null;
    //                targetTextBox.Text = searchText;
    //                textBox.CaretIndex = int.MaxValue;
    //            }

    //            if (string.IsNullOrEmpty(searchText))
    //            {
    //                targetComboBox.Items.Filter = item => true;
    //                targetComboBox.SelectedItem = default(object);
    //            }
    //            else
    //                targetComboBox.Items.Filter = item =>
    //                        item.ToString().StartsWith(searchText, true, CultureInfo.InvariantCulture);

    //            Keyboard.ClearFocus();
    //            Keyboard.Focus(targetTextBox);
    //            targetTextBox.CaretIndex = int.MaxValue;
    //            targetComboBox.IsDropDownOpen = true;
    //        }
    //    };


    //    targetComboBox.SelectionChanged += (o, args) =>
    //    {
    //        var comboBox = o as ComboBox;
    //        if (comboBox?.SelectedItem == null) return;
    //        comboBox.Tag = "Selection";
    //    };
    //}
}

