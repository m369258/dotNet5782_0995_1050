using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        private BlApi.IBl bl = new BlApi.Bl();
        public ProductWindow()
        {
            InitializeComponent();
            cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));  
                btnAddOrUpdateProduct.Content = "Add";
        }


        public ProductWindow(int id)
        {
            InitializeComponent();
            cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));
            BO.Product boProduct = bl.product.GetProduct(id);
            txtID.Text = boProduct.ID.ToString();
            txtName.Text = boProduct.Name;
            txtPrice.Text=boProduct.Price.ToString();
            cbxCategory.SelectedItem = boProduct.Category;
            txtInStock.Text=boProduct.InStock.ToString();

            btnAddOrUpdateProduct.Content = "Update";
        }

        private void btnAddOrUpdateProduct_Click(object sender, RoutedEventArgs e)
        {

            //בדיקה האם הקלטים חוקיים
            //במידה ולא ישלח צסז בוקס
            if (btnAddOrUpdateProduct.Content == "Add")
            {
                bl.product.AddProduct(new BO.Product()
                {
                    ID = int.Parse(txtID.Text),
                    Name = txtName.Text,
                    Category = (BO.Category)(cbxCategory.SelectedItem),
                    Price = double.Parse(txtPrice.Text),
                    InStock = int.Parse(txtInStock.Text)
                });
            }
            else
            {
                BO.Product bop = new BO.Product();
                bop.ID = int.Parse(txtID.Text);
                bop.Name = txtName.Text;
                bop.Category = (BO.Category)(cbxCategory.SelectedItem);
                bop.Price = double.Parse(txtPrice.Text);
                bop.InStock = int.Parse(txtInStock.Text);
                bl.product.UpDateProduct(bop);
            }
            this.Close();
                  }
    }
}
