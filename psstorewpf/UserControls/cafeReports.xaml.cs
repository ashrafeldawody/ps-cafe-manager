using MahApps.Metro.Controls;
using psstorewpf.Classes;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace psstorewpf.UserControls
{
    /// <summary>
    /// Interaction logic for dailyreport.xaml
    /// </summary>
    public partial class cafeReports : UserControl
    {
        DataAccess da = new DataAccess();
        public List<CafeInvoice> listOfInvoices { get; set; }

        public cafeReports()
        {
            InitializeComponent();
            this.DataContext = this;

            startDate.SelectedDateTime = DateTime.Now.AddHours(-12);
            endDate.SelectedDateTime = DateTime.Now;
        }
        public void calculateTotalCost()
        {
            decimal sum = 0;
            foreach (CafeInvoice pInvoice in listOfInvoices)
            {
                sum += pInvoice.cost;
            }
            totalLabel.Content = sum.ToString("0.00");
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            listOfInvoices = da.getDateTimeRange<CafeInvoice>("cafe_invoices", "datetime", (DateTime)startDate.SelectedDateTime, (DateTime)endDate.SelectedDateTime);
            Imported.ItemsSource = listOfInvoices;
            Imported.Items.Refresh();
            calculateTotalCost();

        }

        private void MetroWindow_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.F1 && Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                startDate.IsEnabled = !startDate.IsEnabled;
                endDate.IsEnabled = !endDate.IsEnabled;
            }
        }

        private void export_Click(object sender, RoutedEventArgs e)
        { 


        }


    }
}
