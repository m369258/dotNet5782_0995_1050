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

namespace PL
{
    /// <summary>
    /// Interaction logic for MainCustomerWindow.xaml
    /// </summary>
    public partial class MainCustomerWindow : Window
    {
        BlApi.IBl bl = BlApi.Factory.Get();

        public ObservableCollection<BO.ProductItem?> ProductItem
        {
            get { return (ObservableCollection<BO.ProductItem?>)GetValue(ProductItemProperty); }
            set { SetValue(ProductItemProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ProductItem.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ProductItemProperty =
            DependencyProperty.Register("ProductItem", typeof(ObservableCollection<BO.ProductItem?>), typeof(MainCustomerWindow), new PropertyMetadata(null));

        public MainCustomerWindow()
        {
            InitializeComponent();

            var temp = bl.product.GetCatalog();
            ProductItem = temp == null ? new() : new(temp);
        }
    }
}
