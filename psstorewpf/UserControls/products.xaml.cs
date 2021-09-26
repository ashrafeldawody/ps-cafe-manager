using MahApps.Metro.Controls;
using psstorewpf.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
namespace psstorewpf.UserControls
{
    /// <summary>
    /// Interaction logic for dailyreport.xaml
    /// </summary>
    public partial class ProductsUC : UserControl
    {
        DataAccess da = new DataAccess();
        public List<Items> items { get; set; }

        public ProductsUC()
        {
            items = da.getAll<Items>("items");
            InitializeComponent();
            this.DataContext = this;
            refreshDataView();
        }
        public void refreshDataView()
        {
            items = da.getAll<Items>("items");
            ProductsDataGrid.ItemsSource = items;    
            ProductsDataGrid.Items.Refresh();
            calculateTotalPrice();
        }

        private void addProduct_Click(object sender, RoutedEventArgs e)
        {
            addProduct ap = new addProduct();
            ap.ShowDialog();
        }

        private void refreshProductsList_Click(object sender, RoutedEventArgs e)
        {
            refreshDataView();
        }
        public void calculateTotalPrice()
        {
            decimal sum = 0;
            foreach (Items itm in items)
            {
                sum += itm.totalPrice;
            } 
            totalLabel.Content = sum.ToString("0.00");
        }

        private void removeSelected_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedIndex == -1) return;
            Items i = (Items)ProductsDataGrid.SelectedItem;
            da.DeleteRecord<Items>("items", "name", i.name);
            refreshDataView();
        }

        private void updateProduct_Click(object sender, RoutedEventArgs e)
        {
            if (ProductsDataGrid.SelectedIndex == -1) return;
            Items itm = (Items)ProductsDataGrid.SelectedItem;
            editProduct editWindow = new editProduct();
            editWindow.Id = itm.Id;
            editWindow.productName.Text = itm.name;
            editWindow.productPrice.Value = (double)itm.price;
            editWindow.productQty.Value = (int)itm.qty;
            editWindow.ShowDialog();

        }
    }
}
