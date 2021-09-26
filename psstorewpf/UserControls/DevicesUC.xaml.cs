using MahApps.Metro.Controls;
using MongoDB.Bson;
using psstorewpf.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Text;
using System.Windows;
using System.Windows.Controls;


namespace psstorewpf.UserControls
{
    /// <summary>
    /// Interaction logic for pos.xaml
    /// </summary>
    public partial class DevicesUC : UserControl
    {
        public List<Device> devices { get; set; }

        DataAccess da = new DataAccess();

        public DevicesUC()
        {
            InitializeComponent();
            refreshDataView();
            DataContext = this;
        }
        public void refreshDataView()
        {
            devices = Device.getAll<Device>();
            DevicesDataGrid.ItemsSource = devices;
            DevicesDataGrid.Items.Refresh();
        }
        private void removeSelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void refreshList_Click(object sender, RoutedEventArgs e)
        {
            refreshDataView();
        }

        private void updateSelected_Click(object sender, RoutedEventArgs e)
        {

        }

        private void addDevice_Click(object sender, RoutedEventArgs e)
        {
            addDevice popup = new addDevice();
            popup.ShowDialog();
        }
    }
}
