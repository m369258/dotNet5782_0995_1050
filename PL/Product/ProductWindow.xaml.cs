using System;
using System.Windows;

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
            txtID.IsEnabled = true;
            this.Activated += (s, a) => this.ApplyState();
            this.LocationChanged += (s, a) => this.SetState();
        }


        public ProductWindow(int id)
        {
            InitializeComponent();
            this.Activated += (s, a) => this.ApplyState();
            this.LocationChanged += (s, a) => this.SetState();

            BO.Product boProduct = new BO.Product();
            cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));
            try { boProduct = bl.product.GetProduct(id); }
            catch (BO.InternalErrorException) { MessageBox.Show("מוצר לא קיים"); }
            txtID.Text = boProduct.ID.ToString();
            txtName.Text = boProduct.Name;
            txtPrice.Text = boProduct.Price.ToString();
            cbxCategory.SelectedItem = boProduct.Category;
            txtInStock.Text = boProduct.InStock.ToString();

            btnAddOrUpdateProduct.Content = "Update";
            txtID.IsEnabled = false;
        }

        private void btnAddOrUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            int id, inStock;
            double price;
            if (txtID.Text == "" || txtName.Text == "" || txtPrice.Text == "" || txtInStock.Text == "" || cbxCategory.SelectedIndex == -1)
            {
                MessageBox.Show("אנא מלא את כל השדות");
                return;
            }
            if (!int.TryParse(txtID.Text, out id)) { MessageBox.Show("מזהה לא חוקי"); return; } ;
            if (!double.TryParse(txtPrice.Text, out price)) { MessageBox.Show("מחיר לא חוקי"); return; } ;
            if (!int.TryParse(txtInStock.Text, out inStock)) { MessageBox.Show("כמות במלאי לא חוקית"); return; } ;

            if (btnAddOrUpdateProduct.Content.ToString() == "Add")
            {
                try
                {
                    bl.product.AddProduct(new BO.Product()
                    {
                        ID = id,
                        Name = txtName.Text,
                        Category = (BO.Category)(cbxCategory.SelectedItem),
                        Price = price,
                        InStock = inStock
                    });
                }
                catch { MessageBox.Show("מוצר לא התווסף משום קלט לא חוקי"); }
            }
            else
            {
                BO.Product bop = new BO.Product();
                bop.ID = id;
                bop.Name = txtName.Text;
                bop.Category = (BO.Category)(cbxCategory.SelectedItem);
                bop.Price = price;
                bop.InStock = inStock;
                try { bl.product.UpDateProduct(bop); }
                catch { MessageBox.Show("מוצר לא התווסף משום קלט לא חוקי"); }
            }
            this.Close();
        }
    }
}
