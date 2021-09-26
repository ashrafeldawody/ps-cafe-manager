using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using MongoDB.Bson;
using psstorewpf.Classes;
using System.Drawing.Printing;
using System.Text;
using System.Diagnostics;

namespace psstorewpf.UserControls
{
    /// <summary>
    /// Interaction logic for psdevice.xaml
    /// </summary>
    public partial class psdevice : UserControl
    {

        public static Device ps4;
        public PlayInvoice playInv;
        public CafeInvoice cafeInv = new CafeInvoice();
        public string Currancy = "جنيه";
        public List<Items> items { get; set; }

        private List<Cart> cart = new List<Cart>();

        public decimal total_cost => cafeInv.cost + playInv.cost;
        public double seconds { get; set; }

        DispatcherTimer Unlimitedtimer = new DispatcherTimer();
        DispatcherTimer limitedtimer = new DispatcherTimer();
        DataAccess da = new DataAccess();
        public psdevice(Device dev)
        {
            ps4 = dev;
            playInv = new PlayInvoice();
            items = da.getAll<Items>("items");
            InitializeComponent();
            this.DataContext = this;

        }


        public void setDeviceInfo(Device dev)
        {
            ps4 = dev;
        }

        private void Countup_Tick(object sender, EventArgs e)
        {
            seconds = (DateTime.Now - playInv.startTime).TotalSeconds;
            TimeSpan time = TimeSpan.FromSeconds(seconds);
            string str = time.ToString(@"hh\:mm\:ss");
            timerLabel.Content = str;
            playCostLabel.Content = playInv.cost.ToString("0.00");
            UpdateLabels();

        }

        private void startUnlimitedTime_Click(object sender, RoutedEventArgs e)
        {
            PayButton.IsEnabled = true;
            deleteInvoice.IsEnabled = true;
            toggleUnlimited();
            playInv.startTime = DateTime.Now;
            Unlimitedtimer.Interval = new TimeSpan(0, 0, 1);
            Unlimitedtimer.Tick += new EventHandler(Countup_Tick);
            Unlimitedtimer.Start();
        }
        private void toggleUnlimited()
        {
            if (startUnlimitedTime.Visibility == Visibility.Visible)
            {
                startUnlimitedTime.Visibility = Visibility.Hidden;
                timeCombo.IsEnabled = false;
                startLimitedTime.IsEnabled = false;
                isMulti.IsEnabled = false;
                isNormal.IsEnabled = false;
            }
            else
            {
                startUnlimitedTime.Visibility = Visibility.Visible;
                timeCombo.IsEnabled = true;
                startLimitedTime.IsEnabled = true;
                isMulti.IsEnabled = true;
                isNormal.IsEnabled = true;
                feedbackMSG.Visibility = Visibility.Hidden;

            }

        }
        private void toggleLimited()
        {
            if (startLimitedTime.Visibility == Visibility.Visible)
            {
                startLimitedTime.Visibility = Visibility.Hidden;
                timeCombo.IsEnabled = false;
                startUnlimitedTime.IsEnabled = false;
                isMulti.IsEnabled = false;
                isNormal.IsEnabled = false;

            }
            else
            {
                startLimitedTime.Visibility = Visibility.Visible;
                timeCombo.IsEnabled = true;
                startUnlimitedTime.IsEnabled = true;
                isMulti.IsEnabled = true;
                isNormal.IsEnabled = true;
                feedbackMSG.Visibility = Visibility.Hidden;
            }
        }
        public void appendToCart(Cart newcart)
        {
            foreach (Cart itm in cart)
            {
                if (itm.name == newcart.name)
                {
                    return;
                }

            }
            cart.Add(newcart);
            refreshCafeCart();
        }
        public void refreshCafeCart()
        {
            cafeCart.ItemsSource = cart;
            cafeCart.Items.Refresh();
            cafeInv.cost = calculateCafeTotal();
            cafeCostLabel.Content = cafeInv.cost.ToString("0.00");
            UpdateLabels();
        }

        public decimal calculateCafeTotal()
        {
            decimal total = 0;
            foreach (Cart itm in cart)
            {
                total += itm.total_price;
            }
            return total;

        }
        private void decrementQty()
        {
            foreach (Cart itm in cart)
            {
                da.decrementQty<Items>(itm.name, (int)itm.qty);
            }
        }
        private void StartLimitedTime_Click(object sender, RoutedEventArgs e)
        {

            toggleLimited();
            playInv.startTime = DateTime.Now;
            playInv.endTime = playInv.startTime.AddMinutes(Time.stringTimeToMinutes(timeCombo.SelectedIndex));
            limitedtimer.Interval = new TimeSpan(0, 0, 1);
            limitedtimer.Tick += new EventHandler(Countdown_Tick);
            limitedtimer.Start();
            UpdateLabels();
            PayButton.IsEnabled = true;
            deleteInvoice.IsEnabled = true;
        }
        private void Countdown_Tick(object sender, EventArgs e)
        {
            seconds = (playInv.endTime - DateTime.Now).TotalSeconds;
            if (seconds > 0)
            {
                TimeSpan time = TimeSpan.FromSeconds(seconds);
                string str = time.ToString(@"hh\:mm\:ss");
                timerLabel.Content = str;
            }
            else
            {
                TimeEndWarning TEW = new TimeEndWarning();
                TEW.TitleProp = deviceName.Content + ": انتهي الوقت";
                if (TEW.ShowDialog() == true)
                {
                    playInv.endTime = playInv.endTime.AddMinutes(TEW.minutes);
                }
                else
                {
                    toggleLimited();
                    limitedtimer.Stop();
                    payInvoice();
                    return;
                }
            }
            decimal hours = (decimal)(DateTime.Now - playInv.startTime).TotalHours;
            playCostLabel.Content = playInv.cost.ToString("0.00");
            UpdateLabels();
            Debug.WriteLine(playInv.hoursDiff);
        }

        private void PayButton_Click(object sender, RoutedEventArgs e)
        {
            if (Unlimitedtimer.IsEnabled)
            {
                toggleUnlimited();
                Unlimitedtimer.Stop();
                payInvoice();
            }
            else if (limitedtimer.IsEnabled)
            {
                toggleLimited();
                limitedtimer.Stop();
                payInvoice();
            }
        }

        public void payInvoice()
        {
            //if (playCost < 2)
            //{
            //    MessageBox.Show("وقت اللعب قليل جدا، يرجي الغاء الفاتوره");
            //    return;
            //}

            playInv.endTime = DateTime.Now;
            
            if (cart.Count > 0)
            {
                da.insert("cafe_invoices", new CafeInvoice()
                {
                    device_name = deviceName.Content.ToString(),
                    datetime = DateTime.Now,
                    items = cartToBson(),
                    cost = cafeInv.cost,
                });

            }
            decrementQty();
            if (printInvoiceCheckbox.IsChecked == true)
            {
                PrintInvoice();
            }
            resetAll();

        }
        private BsonArray cartToBson()
        {
            BsonArray arr = new BsonArray();
            foreach (Cart itm in cart)
            {
                BsonDocument doc = new BsonDocument();
                doc.Add("name", itm.name);
                doc.Add("price", itm.price.ToString());
                doc.Add("quantity", itm.qty.ToString());
                doc.Add("total_price", itm.total_price.ToString());
                arr.Add(doc);
            }
            return arr;
        }

        private void UpdateLabels()
        {
            playCostLabel.Content = playInv.cost.ToString("0.00");
            cafeCostLabel.Content = cafeInv.cost.ToString("0.00");
            totalCostLabel.Content = Math.Round(total_cost).ToString("0.00");
            paidAmount.Text = Math.Round(total_cost).ToString("0.00");
        }

        public void PrintInvoice()
        {

            PrintDocument pd = new PrintDocument();
            pd.DefaultPageSettings.PaperSize = new PaperSize("", 300, 600);
            pd.PrintPage += new PrintPageEventHandler(OpenThermal);
            Console.WriteLine(pd.ToString());
            pd.Print();


        }

        private void OpenThermal(object sender, PrintPageEventArgs e)
        {
            var sb = new StringBuilder();

            sb.AppendLine("جيم أون".PadLeft(33));
            sb.AppendLine(" ");
            sb.AppendLine("التاريخ: " + DateTime.Now.ToLongDateString());
            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine("اللعب".PadLeft(33));

            sb.Append("الجهاز".PadRight(8));
            sb.AppendLine(": بلايستيشن 4 - " + deviceName.Content);

            sb.Append("نوع اللعب".PadRight(8));
            sb.AppendLine(": " + (isMulti.IsChecked == true ? "مالتي" : "عادي"));


            sb.Append("سعر الساعة".PadRight(8));
            sb.AppendLine(": " + (isMulti.IsChecked == true ? ps4.Multi_Price : ps4.pricePerHour) + " جنيه");

            sb.Append("وقت البدء".PadRight(8));
            sb.AppendLine(": " + playInv.startTime.ToShortTimeString());

            sb.Append("وقت الإنتهاء".PadRight(8));
            sb.AppendLine(": " + playInv.endTime.ToShortTimeString());


            sb.Append("المدة".PadRight(8));
            sb.AppendLine(": " + playInv.duration + " دقيقة");

            sb.Append("تكلفة اللعب".PadRight(8));
            sb.AppendLine(": " + playInv.cost.ToString("0.00") + " جنيه");
            sb.AppendLine("=".PadRight(60, '='));

            sb.AppendLine("الكافيه".PadLeft(33));

            sb.AppendLine("-".PadRight(60, '-'));
            foreach (Cart itm in cart)
            {
                sb.Append(itm.name.PadLeft(10));
                sb.Append((itm.qty + "×" + itm.price).PadLeft(13));
                sb.AppendLine(itm.total_price.ToString("0.00").PadLeft(13));
            }
            sb.Append("تكلفة اللعب".PadRight(8));
            sb.AppendLine(": " + playInv.cost.ToString("0.00") + " جنيه");

            sb.AppendLine("=".PadRight(50, '='));
            sb.Append(" المجموع:".PadLeft(20));
            sb.AppendLine(Math.Round(total_cost).ToString("0.00") + "جنيه");

            sb.Append(" المدفوع:".PadLeft(20));
            sb.AppendLine(decimal.Parse(paidAmount.Text).ToString("0.00") + "جنيه");

            sb.AppendLine("=".PadRight(50, '='));


            var printText = new PrintText(sb.ToString(), new Font(System.Drawing.FontFamily.GenericMonospace, 9, System.Drawing.FontStyle.Bold));
            Graphics graphics = e.Graphics;
            int startX = 0;
            int startY = 100;
            int Offset = 20;
            System.Drawing.Image newImage = Properties.Resources.LOGO;
            graphics.DrawImage(newImage,new Rectangle(50, 0, newImage.Width, newImage.Height));
            graphics.DrawString(printText.Text.ToString(), new Font("Arial", 11, System.Drawing.FontStyle.Bold),
                                new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
        }

        private void deleteInvoice_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new prompt();
            if (dialog.ShowDialog() == true)
            {
                if (Unlimitedtimer.IsEnabled)
                {
                    toggleUnlimited();
                    playInv.endTime = DateTime.Now;
                    Unlimitedtimer.Stop();
                }
                else if (limitedtimer.IsEnabled == true)
                {
                    toggleLimited();
                    limitedtimer.Stop();
                }
                if (total_cost < 1)
                {
                    resetAll(true);
                    return;
                }
                cartToBson();
            //    var doc = new BsonDocument {
            //    { "device_name", deviceName.Content.ToString() },
            //    { "startTime", playInv.startTime },
            //    { "endTime", endTime },
            //    { "duration", duration.ToString() },
            //    { "type", isMulti.IsChecked == true ? "مالتي" : "عادي" },
            //    { "pricePerHour", isMulti.IsChecked == true ? ps4.Multi_Price : ps4.pricePerHour },
            //    { "items", cartToBson() },
            //    { "playCost", playCost.ToString("0.00") },
            //    { "cancelation_reason", dialog.ResponseText },
            //};
            //    da.insert("deleted_invoices", doc);
                resetAll(true);
            }

        }
        public void resetAll(Boolean delete = false)
        {
            playCostLabel.Content = "0.0";
            cafeCostLabel.Content = "0.0";
            totalCostLabel.Content = "0.0";
            timerLabel.Content = "00:00:00";
            PayButton.IsEnabled = false;
            deleteInvoice.IsEnabled = false;
            cafeItems.SelectedIndex = -1;
            isNormal.IsChecked = true;
            cart.Clear();
            refreshCafeCart();
            if (delete)
            {
                feedbackMSG.Content = "تم حذف الفاتوره بنجاح";
                feedbackMSG.Foreground = System.Windows.Media.Brushes.Red;
                feedbackMSG.Visibility = Visibility.Visible;
            }
            else
            {
                feedbackMSG.Content = "تم تسجيل الفاتوره بنجاح";
                feedbackMSG.Foreground = System.Windows.Media.Brushes.Green;
                feedbackMSG.Visibility = Visibility.Visible;
            }
        }


        private void cartPlus_Click(object sender, RoutedEventArgs e)
        {
            var __cart = (Cart)((Button)sender).DataContext;

            __cart.qty++;
            refreshCafeCart();
        }

        private void cartMinus_Click(object sender, RoutedEventArgs e)
        {
            var __cart = (Cart)((Button)sender).DataContext;
            if(__cart.qty > 1)
                __cart.qty--;
            else
            {
                cart.RemoveAll(x => x.name == __cart.name);
            }

            refreshCafeCart();
        }

        private void cafeItems_DropDownClosed(object sender, EventArgs e)
        {
            if (cafeItems.SelectedIndex >= 0)
            {
                Items item = (Items)cafeItems.SelectedItem;
                Cart cart = new Cart { name = item.name, qty = 1, price = item.price };
                appendToCart(cart);
            }
        }


    }
}