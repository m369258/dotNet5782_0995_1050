using System;
using System.Windows;

namespace PL.Product
{
    /// <summary>
    /// Interaction logic for ProductWindow.xaml
    /// </summary>
    public partial class ProductWindow : Window
    {
        //A private variable to access the logic layer
        private BlApi.IBl bl = new BlApi.Bl();

        /// <summary>
        /// A constructive action for the state of adding a product
        /// </summary>
        public ProductWindow()
        {
            InitializeComponent();
            cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));
            btnAddOrUpdateProduct.Content = "הוספה";
            txtID.IsEnabled = true;
        }

        /// <summary>
        /// Constructive action for product update status
        /// </summary>
        /// <param name="id">ID product</param>
        public ProductWindow(int id)
        {
            InitializeComponent();

            //Product request by ID from the logical layer
            BO.Product boProduct = new BO.Product();
            try { boProduct = bl.product.GetProduct(id); }
            catch (BO.InternalErrorException) { MessageBox.Show("מוצר לא קיים"); }

            //Place all controls according to the data
            txtID.Text = boProduct.ID.ToString();
            txtName.Text = boProduct.Name;
            txtPrice.Text = boProduct.Price.ToString();
            cbxCategory.SelectedItem = boProduct.Category;
            txtInStock.Text = boProduct.InStock.ToString();

            //The name of the selected category
            cbxCategory.ItemsSource = Enum.GetValues(typeof(BO.Category));

            btnAddOrUpdateProduct.Content = "עידכון";

            //Locks the option to change ID
            txtID.IsEnabled = false;
        }

        /// <summary>
        /// A function that updates or adds a product
        /// </summary>
        /// <param name="sender">Add or update button</param>
        /// <param name="e">More information about the button</param>
        private void btnAddOrUpdateProduct_Click(object sender, RoutedEventArgs e)
        {
            int id, inStock;
            double price;
            //A message will be displayed if one of the fields is empty
            if (txtID.Text == "" || txtName.Text == "" || txtPrice.Text == "" || txtInStock.Text == "" || cbxCategory.SelectedIndex == -1)
            {
                MessageBox.Show("אנא מלא את כל השדות");
                return;
            }

            //Checking the correctness of the information received
            if (!int.TryParse(txtID.Text, out id)) { MessageBox.Show("מזהה לא חוקי"); return; } ;
            if (!double.TryParse(txtPrice.Text, out price)) { MessageBox.Show("מחיר לא חוקי"); return; } ;
            if (!int.TryParse(txtInStock.Text, out inStock)) { MessageBox.Show("כמות במלאי לא חוקית"); return; } ;

            //In case of addition, a product will be added to the logical layer
            if (btnAddOrUpdateProduct.Content.ToString() == "הוספה")
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
                catch { MessageBox.Show("מוצר לא התווסף משום קלט לא חוקי");}
            }
            //In case of an update, the product will be updated to the logical layer
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
