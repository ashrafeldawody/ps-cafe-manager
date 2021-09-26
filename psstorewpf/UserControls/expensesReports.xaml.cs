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
    public partial class expensesReports : UserControl
    {
        DataAccess da = new DataAccess();
        public List<Expense> listOfExpenses { get; set; }

        public expensesReports()
        {
            InitializeComponent();
            this.DataContext = this;

            startDate.SelectedDateTime = DateTime.Now.AddHours(-12);
            endDate.SelectedDateTime = DateTime.Now;
        }
        public void calculateTotalCost()
        {
            decimal sum = 0;
            foreach (Expense exp in listOfExpenses)
            {
                sum += decimal.Parse(exp.amount);
            }
            totalLabel.Content = sum.ToString("0.00");
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            listOfExpenses = da.getDateTimeRange<Expense>("expenses", "datetime", (DateTime)startDate.SelectedDateTime, (DateTime)endDate.SelectedDateTime);
            Imported.ItemsSource = listOfExpenses;
            Imported.Items.Refresh();
            calculateTotalCost();

        }

    }
}
