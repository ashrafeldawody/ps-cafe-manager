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
    public partial class addDevice : MetroWindow
    {
        public addDevice()
        {
            InitializeComponent();
        }
        public string name
        {
            get { return DeviceName.Text; }
            set { DeviceName.Text = value; }
        }
        public decimal normalPrice
        {
            get { return (decimal)NormalPrice.Value; }
            set { NormalPrice.Value = (double)value; }
        }
        public decimal multiPrice
        {
            get { return (decimal)MultiPrice.Value; }
            set { MultiPrice.Value = (double)value; }
        }
        

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            new Device(name, normalPrice, multiPrice).insert();
            MessageBox.Show("تم اضافة الجهاز بنجاح");
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
