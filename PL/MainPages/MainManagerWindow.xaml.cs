using PL.Order;
using PL.Product;
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

namespace PL.MainPages
{
    /// <summary>
    /// Interaction logic for MainManagerWindow.xaml
    /// </summary>
    public partial class MainManagerWindow : Window
    {

        //    public string name { get; set; }
        //    /// <summary>
        //    /// ctor of manager window
        //    /// </summary>
        //    public MainManagerWindow(string name)
        //    {
        //        this.name = name;
        //        InitializeComponent();
        //    }
        //    /// <summary>
        //    /// open product list window
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void btnProduct_Click(object sender, RoutedEventArgs e) => new ProductForListWindow().ShowDialog();
        //    /// <summary>
        //    /// open orders list window
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void btnProducts_Click(object sender, RoutedEventArgs e) => new OrderForListWindow().ShowDialog();
        /////// <summary>
        /////// open order tracking window
        /////// </summary>
        /////// <param name="sender"></param>
        /////// <param name="e"></param>
        ////private void btnOrderTracking_Click(object sender, RoutedEventArgs e) => new OrderTrackingWindow().ShowDialog();
        ////    /// <summary>
        ////    /// open signing up window
        ////    /// </summary>
        ////    /// <param name="sender"></param>
        ////    /// <param name="e"></param>
        ////    private void btnAddManager_Click(object sender, RoutedEventArgs e) => new SignUpWindow(BO.Position.Manager).ShowDialog();
        //    /// <summary>
        //    /// close the window
        //    /// </summary>
        //    /// <param name="sender"></param>
        //    /// <param name="e"></param>
        //    private void Button_Click(object sender, RoutedEventArgs e)
        //    {
        //        this.Close();
        //        new LogInWindow().ShowDialog();
        //    }

        public string name { get; set; }

        public MainManagerWindow()
        {
            InitializeComponent();
        }
        public MainManagerWindow(string userName)
        {
            InitializeComponent();
           this.name = userName;
        }
        private void btnProducts_Click(object sender, RoutedEventArgs e) => new ProductForListWindow().ShowDialog();

        private void btnOrders_Click(object sender, RoutedEventArgs e) => new OrderForListWindow().ShowDialog();
    }
}
