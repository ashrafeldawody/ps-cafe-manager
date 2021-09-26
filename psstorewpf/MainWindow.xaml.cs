using MahApps.Metro.Controls;
using psstorewpf.Classes;
using psstorewpf.UserControls;
using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace psstorewpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public List<Device> devices { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            devices = Device.getAll<Device>();
            foreach (Device device in devices)
            {
                psdevice tempdev = new psdevice(device);
                stackPnl.Children.Add(tempdev);
            }
            
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void CloseApp_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(1);

        }

        private void cafeInvoiceButton_Click(object sender, RoutedEventArgs e)
        {

            POS posWindow = new POS();
            posWindow.ShowDialog();
        }

        private void withdrawButton_Click(object sender, RoutedEventArgs e)
        {
            withdraw withdrawWindow = new withdraw();
            withdrawWindow.ShowDialog();
        }

        private void controllersReports_Click_1(object sender, RoutedEventArgs e)
        {
            controllersReports cr = new controllersReports();
            cr.ShowDialog();
        }


        private void controlPanel_Click(object sender, RoutedEventArgs e)
        {
            controlPanel cp = new controlPanel();
            cp.ShowDialog();
        }
    }
}
