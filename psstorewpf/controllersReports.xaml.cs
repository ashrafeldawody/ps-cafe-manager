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
    public partial class controllersReports : MetroWindow
    {
        public controllersReports()
        {
            InitializeComponent();
        }
        public string compain
        {
            get { return ComplainTextBox.Text; }
            set { ComplainTextBox.Text = value; }
        }
        public string controllerNum
        {
            get { return ControllerNumberTextBox.Text; }
            set { ControllerNumberTextBox.Text = value; }
        }
        DataAccess da = new DataAccess();

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            var controller_report = new BsonDocument {
                { "datetime", DateTime.Now },
                { "controller_num", controllerNum },
                { "complain", compain },
            };

            da.insert("controller_reports", controller_report);
            MessageBox.Show("تم تسجيل المشكلة بنجاح");
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
