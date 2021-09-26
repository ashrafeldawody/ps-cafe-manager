using MahApps.Metro.Controls;
using psstorewpf.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace psstorewpf
{
    /// <summary>
    /// Interaction logic for dailyreport.xaml
    /// </summary>
    public partial class controlPanel : MetroWindow
    {
        DataAccess da = new DataAccess();

        public controlPanel()
        {
            InitializeComponent();
            this.DataContext = this;
        }

    }
}
