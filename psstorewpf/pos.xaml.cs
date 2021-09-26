using MahApps.Metro.Controls;
using MongoDB.Bson;
using psstorewpf.Classes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
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
    /// Interaction logic for pos.xaml
    /// </summary>
    public partial class POS : MetroWindow
    {
        private List<Cart> _cart = new List<Cart>();
        public List<Cart> cart
        {
            get { return _cart; }
            set { _cart = value; }
        }
        public List<Items> items { get; set; }

        public decimal cost { get; set; }
        DataAccess da = new DataAccess();

        public POS()
        {
            items = da.getAll<Items>("items");
            InitializeComponent();
            DataContext = this;
        }

        private void cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Pay_Click(object sender, RoutedEventArgs e)
        {
            if (cafeCart.Items.Count < 1) {
                MessageBox.Show("لا يوجد منتجات في سله المشتريات");
                return;
            }
            da.insert("cafe_invoices", new CafeInvoice()
            {
                datetime = DateTime.Now,
                items = cartToBson(),
                cost = cost,

            });
            decrementQty();
            MessageBox.Show("تم اضافة الفاتورة بنجاح");

            if (printInvoiceCheckbox.IsChecked == true)
                PrintInvoice();
            this.Close();

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
            sb.AppendLine(" ");
            sb.AppendLine("التاريخ: " + DateTime.Now.ToLongDateString());

            sb.AppendLine("=".PadRight(60, '='));
            foreach (Cart itm in cart)
            {
                sb.Append(itm.name.PadLeft(10));
                sb.Append((itm.qty + "×" + itm.price).PadLeft(13));
                sb.AppendLine(itm.total_price.ToString("0.00").PadLeft(13));
            }
            sb.AppendLine("=".PadRight(60, '='));
            sb.AppendLine("=".PadRight(60, '='));

            sb.Append(" المجموع:".PadLeft(20));
            sb.AppendLine(cost.ToString("0.00") + " جنيه ");

            sb.AppendLine("=".PadRight(50, '='));


            var printText = new PrintText(sb.ToString(), new Font(System.Drawing.FontFamily.GenericMonospace, 9, System.Drawing.FontStyle.Bold));
            Graphics graphics = e.Graphics;
            int startX = 0;
            int startY = 100;
            int Offset = 20;
            System.Drawing.Image newImage = Properties.Resources.LOGO;
            graphics.DrawImage(newImage, new System.Drawing.Rectangle(50, 0, newImage.Width, newImage.Height));
            graphics.DrawString(printText.Text.ToString(), new Font("Arial", 11, System.Drawing.FontStyle.Bold),
                                new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
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
        private void decrementQty()
        {
            foreach (Cart itm in cart)
            {
                da.decrementQty<Items>(itm.name, (int)itm.qty);
            }
        }
        public void refreshCafeCart()
        {
            cafeCart.ItemsSource = cart;
            cafeCart.Items.Refresh();
            cost = calculateCafeTotal();
            cafeCostLabel.Content = cost.ToString("0.00");
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

        private void cafeItems_DropDownClosed(object sender, EventArgs e)
        {
            if(cafeItems.SelectedIndex >= 0) { 
            Items item = (Items)cafeItems.SelectedItem;
            Cart cart = new Cart { name = item.name, qty = 1, price = item.price };
            appendToCart(cart);
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
            if (__cart.qty > 1)
                __cart.qty--;
            else
            {
                cart.RemoveAll(x => x.name == __cart.name);
            }

            refreshCafeCart();
        }
    }
}
