using MongoDB.Bson;
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
    /// Interaction logic for receipt.xaml
    /// </summary>
    public partial class receipt : Window
    {
        public BsonDocument bill { get; set; }
        public receipt()
        {
            InitializeComponent();
            MessageBox.Show(bill.GetValue("startTime").ToString());
        }
    }
}
