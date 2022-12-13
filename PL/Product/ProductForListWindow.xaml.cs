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
            CategorySelector.ItemsSource = Enum.GetValues(typeof(BO.Category));
        }

        private void CategorySelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           BO.Category  currCategorySelect = (BO.Category)CategorySelector.SelectedItem;
            
            ProductListview.ItemsSource=bl.product.GetListOfProducts((int)currCategorySelect);
        }
    }
}
