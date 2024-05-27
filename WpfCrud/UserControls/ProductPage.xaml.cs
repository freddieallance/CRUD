using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WpfCrud.Models;
using WpfCrud.Services;

namespace WpfCrud.UserControls
{
    public partial class ProductPage : UserControl
    {
        //selectedProduct is the object referenced for all CRUD operations except for Read
        private Product selectedProduct = new Product();
        //Im using an ObservableCollection because it inherits InotifyPropertyChanged
        private ObservableCollection<Product> products = new ObservableCollection<Product>();

        public ProductPage()
        {
            InitializeComponent();
            //Reads on initialization
            GetProducts();
        }

        private void GetProducts() //retrieves all the Products
        {
            products = DbServices.productsDbService.Select();
            ProductsGridView.ItemsSource = products;
        }

        //This handler is fired when a user clicks on a row on the DataGrid and makes that product the current reference
        private void ProductsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductsGridView.SelectedItem is Product selected)
            {
                selectedProduct = selected;
                NameTextBox.Text = selectedProduct.Name;
                PriceTextBox.Text = selectedProduct.Price.ToString();
                ReleaseDatePicker.SelectedDate = selectedProduct.ReleaseDate;
            }
        }

        private void ReadProductsButton_Click(object sender, RoutedEventArgs e)
        {
            GetProducts();
        }

        //pulls the data from the GUI and updates the properties of the product to be referenced in CRUD Operations
        private void UpdateSelectedProduct()
        {
            selectedProduct.Name = NameTextBox.Text;
            if (double.TryParse(PriceTextBox.Text, out double price))
            {
                selectedProduct.Price = price;
            }
            selectedProduct.ReleaseDate = ReleaseDatePicker.SelectedDate ?? DateTime.Now;
        }

        //Attempts to create a new product in MySQL
        private void CreateProductButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelectedProduct();
            if (DbServices.productsDbService.Insert(selectedProduct))
            {
                MessageBox.Show("Product Created!");
            }
            else
            {
                MessageBox.Show("An error occurred while creating your Product.");
            }
        }

        //Updates a row in MySQL
        private void UpdateProductButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelectedProduct();
            if (DbServices.productsDbService.Update(selectedProduct))
            {
                MessageBox.Show("Product Updated!");
            }
            else
            {
                MessageBox.Show("An error occurred while updating your Product.");
            }
        }

        //Deletes a Product
        private void DeleteProductButton_Click(object sender, RoutedEventArgs e)
        {
            UpdateSelectedProduct();

            if (DbServices.productsDbService.Delete(selectedProduct))
            {
                MessageBox.Show("Product Deleted!");
            }
            else
            {
                MessageBox.Show("An error occurred while deleting your Product.");
            }
        }
    }
}
