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

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for ProductForListWindow.xaml
    /// </summary>
    public partial class ProductForListWindow : Window
    {
        private BlApi.IBl bl = new BlApi.Bl();
        public ProductForListWindow()
        {
            InitializeComponent();
            ProductListview.ItemsSource = bl.product.GetListOfProducts();
            cmxCategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
            btnClear.Visibility = Visibility.Hidden;
        }


        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(cmxCategorySelector.SelectedIndex!=-1)
            {
                BO.Category currCategorySelect = (BO.Category)cmxCategorySelector.SelectedItem;
                ProductListview.ItemsSource = bl.product.GetListOfProducts((int)currCategorySelect);
                btnClear.Visibility = Visibility.Visible;
            }  
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ProductListview.ItemsSource = bl.product.GetListOfProducts();
            cmxCategorySelector.SelectedIndex = -1;
            btnClear.Visibility = Visibility.Hidden;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new ProductWindow().ShowDialog();
            ProductListview.ItemsSource = bl.product.GetListOfProducts();
        }

        private void ProductListview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductListview.SelectedIndex != -1)
            {
                new ProductWindow(((BO.ProductForList)ProductListview.SelectedItem).ID).ShowDialog();
                ProductListview.SelectedIndex = -1;
                ProductListview.ItemsSource = bl.product.GetListOfProducts();
            }

        }
    }
}
