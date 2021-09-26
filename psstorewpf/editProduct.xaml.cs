using MongoDB.Bson;
using psstorewpf.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using MahApps.Metro.Controls;
namespace psstorewpf
{
    /// <summary>
    /// Interaction logic for prompt.xaml
    /// </summary>
    public partial class editProduct : MetroWindow
    {

        public string Id {get; set;}
        public string name
        {
            get { return productName.Text; }
            set { productName.Text = value; }
        }
        public decimal price
        {
            get { return (decimal)productPrice.Value; }
            set { productPrice.Value = (double)value; }
        }
        public decimal qty
        {
            get { return (decimal)productQty.Value; }
            set { productQty.Value = (double)value; }
        }

        DataAccess da = new DataAccess();
        public editProduct()
        {
            InitializeComponent(); 
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            da.UpdateRecord("items",Id,new Items(name, price,(int)qty));
            MessageBox.Show("تم تعديل المنتج بنجاح");
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closed(object sender, EventArgs e)
        {

        }
    }
}
