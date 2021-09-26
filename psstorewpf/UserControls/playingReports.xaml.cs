using psstorewpf.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace psstorewpf.UserControls
{
    /// <summary>
    /// Interaction logic for dailyreport.xaml
    /// </summary>
    public partial class playingReports : UserControl
    {
        DataAccess da = new DataAccess();
        public List<PlayInvoice> listOfInvoices { get; set; }

        public playingReports()
        {
            //listOfInvoices = da.getAll<PlayInvoice>("play_invoices");
            InitializeComponent();
            this.DataContext = this;
            startDate.SelectedDateTime = DateTime.Now.AddHours(-12);
            endDate.SelectedDateTime = DateTime.Now;
        }
        public void calculateTotalCost()
        {
            decimal sum = 0;
            foreach (PlayInvoice pInvoice in listOfInvoices)
            {
                sum += decimal.Parse(pInvoice.paid);
            }
            totalLabel.Content = sum.ToString("0.00");
        }
        public void calculateTotalDuration()
        {
            int sum = 0;
            foreach (PlayInvoice pInvoice in listOfInvoices)
            {
                sum += pInvoice.duration;
            }
            string formattedDuration = string.Format("{0:D2}:{1:D2}", (int)sum / 60, (int)sum % 60);
            totalDurationLabel.Content = formattedDuration;
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {

            listOfInvoices = da.getDateTimeRange<PlayInvoice>("play_invoices", "startTime", (DateTime)startDate.SelectedDateTime, (DateTime)endDate.SelectedDateTime);
            Debug.WriteLine(listOfInvoices);
            Imported.ItemsSource = listOfInvoices;
            Imported.Items.Refresh();
            calculateTotalCost();
            calculateTotalDuration();

        }

        private void MetroWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1 && Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.RightCtrl))
            {
                startDate.IsEnabled = !startDate.IsEnabled;
                endDate.IsEnabled = !endDate.IsEnabled;
            }
        }
    }
}
