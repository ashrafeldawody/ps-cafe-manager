using MahApps.Metro.Controls;
using psstorewpf.Classes;
using System;
using System.Collections.Generic;
using System.Media;
using System.Windows;

namespace psstorewpf
{
    /// <summary>
    /// Interaction logic for prompt.xaml
    /// </summary>
    public partial class TimeEndWarning : MetroWindow
    {
        SoundPlayer snd;
        public TimeEndWarning()
        {
            InitializeComponent();
            snd = new SoundPlayer(Properties.Resources.alert);
            snd.Play();

        }
        public string TitleProp
        {
            get { return title.Text; }
            set { title.Text = value; }
        }
        public string timeAdded
        {
            get { return timeCombo.Text; }
            set { timeCombo.Text = value; }
        }
        public double minutes
        {
            get { return Time.stringTimeToMinutes(timeCombo.SelectedIndex); }
        }

        private void OKButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            DialogResult = true;
            snd.Stop();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close();
            snd.Stop();
        }
    }
}
