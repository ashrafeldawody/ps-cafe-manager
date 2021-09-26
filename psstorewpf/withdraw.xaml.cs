using MahApps.Metro.Controls;
using MongoDB.Bson;
using psstorewpf.Classes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace psstorewpf
{
    /// <summary>
    /// Interaction logic for prompt.xaml
    /// </summary>
    public partial class withdraw : MetroWindow
    {
        public withdraw()
        {
            InitializeComponent();
        }
        public string Type
        {
            get { return TypeTextBox.Text; }
            set { TypeTextBox.Text = value; }
        }
        public string Reason
        {
            get { return ReasonTextBox.Text; }
            set { ReasonTextBox.Text = value; }
        }
        public string Amount
        {
            get { return AmountTextBox.Text; }
            set { AmountTextBox.Text = value; }
        }
        DataAccess da = new DataAccess();

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            da.insert("expenses", new Expense(Type,Reason,Amount));
            MessageBox.Show("تم تسجيل المعامله بنجاح");
            this.Close();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
