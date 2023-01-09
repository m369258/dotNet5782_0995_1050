using PL.Order;


using BO;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
using System.Xml;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class MainCustomerWindow : Window
    {
        /// <summary>
        /// Bl object to have an access to the Bl functions
        /// </summary>
        private BlApi.IBl bl = BlApi.Factory.Get();

       // public BO.Cart cart = new BO.Cart();
        /// <summary>
        /// ctor for customer window
        /// </summary>
        /// <param name="cart">the cart of the user</param>
        public MainCustomerWindow()
        {
            var cart = bl.product.GetCatalog();
            InitializeComponent();
            try
            {
                catalog.ItemsSource = bl.product.GetListOfProducts();
            }
            //catch (BO.BlNullPropertyException ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //catch (BO.BlWrongCategoryException ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            // prod = new ObservableCollection<BO.ProductForList>(bl.Product.ListOfProducts().OrderBy(x => x?.ID));
            // this.DataContext = prod;
        }
        /// <summary>
        /// select an item and show its details
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListView_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            BO.ProductForList? prod = (BO.ProductForList?)(catalog.SelectedItem);
            try
            {
                //open the window to update the product which was selected
               // ProductForCustomerWindow prodWin = new ProductForCustomerWindow(prod?.ID ?? throw new NullReferenceException("You have to choose a product to update!"), cart);
              //  prodWin.ShowDialog();
            }
            //catches for the exception which mighgt be thrown from the ProductDetailsForManager function
            catch (NullReferenceException ex)
            {
                MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// button for opening cart window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void Button_Click(object sender, RoutedEventArgs e) => new CartWindow(cart).ShowDialog();
        /// <summary>
        /// select the catgory to show
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            BO.Category c;
            string? strCat = (sender as TextBlock)?.Text;
            try
            {
                if (strCat != "Popular")
                {
                    Enum.TryParse(strCat, out c);
                    catalog.ItemsSource = bl.product.GetListOfProducts((int)c);
                }
                else
                    catalog.ItemsSource = bl.product.GetListOfProducts();
            }
            //catch (BO.BlNullPropertyException ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //catch (BO.BlWrongCategoryException ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// reset for the page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                catalog.ItemsSource = bl.product.GetListOfProducts();
            }
            //catch (BO.BlNullPropertyException ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            //catch (BO.BlWrongCategoryException ex)
            //{
            //    MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            //}
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ERROR:(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        /// <summary>
        /// open my orders window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

       // private void menuOrders_Click(object sender, RoutedEventArgs e) => new OrderWindow(cart.CustomerEmail).ShowDialog();


       // private void menuTracking_Click(object sender, RoutedEventArgs e) => new OrderTrackinkWindow().ShowDialog();


        private void menuLogOut_Click(object sender, RoutedEventArgs e) => this.Close();
    }
}
