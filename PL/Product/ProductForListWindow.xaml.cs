using System;
using System.Windows;
using System.Windows.Controls;

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
            this.Activated += (s, a) => this.ApplyState();
            this.LocationChanged += (s, a) => this.SetState();

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
