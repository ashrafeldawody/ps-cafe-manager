using MahApps.Metro.Controls;
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
    public partial class prompt : MetroWindow
    {
        public prompt()
        {
            InitializeComponent();
        }
        public string ResponseText
        {
            get { return ResponseTextBox.Text; }
            set { ResponseTextBox.Text = value; }
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if(ResponseText.Length < 4)
            {
                MessageBox.Show("يرجي ادخال سبب الحذف");
                return;
            }
            DialogResult = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
