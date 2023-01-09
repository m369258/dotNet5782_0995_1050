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
        public MainManagerWindow()
        {
            InitializeComponent();
        }

        private void btnProducts_Click(object sender, RoutedEventArgs e) => new ProductForListWindow().ShowDialog();

        private void btnOrders_Click(object sender, RoutedEventArgs e)=>new OrderForListWindow().ShowDialog();
    }
}
