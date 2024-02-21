using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.VisualBasic;

namespace barberproject
{
    /// <summary>
    /// Interaction logic for Invoice_search.xaml
    /// </summary>
    public partial class Invoice_search : Window
    {
        barberEntities1 db = new barberEntities1();
        public bool check1 = false;
        public Invoice_search()
        {
            InitializeComponent();
        }
        public class Itemm
        {
            public int? id { get; set; }
            public string descriptio { get; set; }
            public int? price { get; set; }
        }
        public class Invoice_Itemm
        {
            public int? Invoice_id { get; set; }
            public string Worker_name { get; set; }
            public double? Amount { get; set; }
            public double? Discount { get; set; }
            public DateTime? Created_at { get; set; }


        }
        private void search_Click(object sender, RoutedEventArgs e)
        {

            if (search_date.IsChecked == false && search_number.IsChecked == false)
            {
                MessageBox.Show("برجاء اختيار طريقة البحث ");
                return;
            }
            if (search_date.IsChecked == true && search_number.IsChecked == true)
            {
                MessageBox.Show("برجاء اختيار طريقة بحث واحدة ");
                return;
            }

            if (search_number.IsChecked == true)
            {
                if (combo_data.Text != "")
                {
                    if (System.Text.RegularExpressions.Regex.IsMatch(combo_data.Text, "[^0-9]"))
                    {
                        MessageBox.Show("برجاء ادخال رقم الفاتورة بصيغة صحيحة ");
                        combo_data.Text = string.Empty;
                        return;
                    }
                    int num = Convert.ToInt32(combo_data.Text);
                    List<Itemm> mmm = new List<Itemm>();
                    var check = db.invoices.Where(g => g.id == num && g.deleted == null).ToList();
                    if (check.Count == 0)
                    {
                        MessageBox.Show("لا توجد بيانات لهذه الفاتورة برجاء اختيار الرقم المناسب");
                        return;
                    }
                    else
                    {
                        if (check1 == true)
                        {
                            list_x.Items.Clear();
                            check1 = false;

                        }

                        stack_visibie.Visibility = Visibility.Visible;
                        stack_visibie1.Visibility = Visibility.Hidden;
                        invoice_id.Text = check[0].id.ToString();
                        date_text.Text = check[0].created_at.ToString();
                        wname_text.Text = check[0].worker_.name;
                        amount_text.Text = check[0].amount.ToString();
                        discount_text.Text = check[0].dicount.ToString();
                        if (check[0].dicount == null)
                            total_text.Text = Convert.ToString((double)(check[0].amount));
                        else total_text.Text = Convert.ToString((double)(check[0].amount - check[0].dicount));



                        List<invoice_items> n = db.invoice_items.Where(m => m.invoice_id == num).ToList();

                        for (int i = 0; i < n.Count(); i++)
                        {
                            int c = n[i].item_id;
                            var x = db.price_List.Where(f => f.ID == c).ToList();


                            mmm.Add(new Itemm { id = x[0].ID, descriptio = x[0].description, price = x[0].Price });
                            list_x.Items.Add(mmm[i]);

                        }
                        check1 = true;
                    }


                }
                else
                {
                    MessageBox.Show("برجاء ادخال رقم الفاتورة ");
                }




            }

            if (search_date.IsChecked == true)
            {
                if (invoice_date.Text != "")
                {
                    double? total = 0;
                    var year = invoice_date.SelectedDate.Value.Year.ToString();
                    var monh = invoice_date.SelectedDate.Value.Month.ToString();
                    var day = invoice_date.SelectedDate.Value.Day.ToString();
                    var check = db.invoices.Where(d => d.created_at.Year.ToString() == year && d.created_at.Month.ToString() == monh && d.created_at.Day.ToString() == day && d.deleted == null).ToList();
                    if (check.Count == 0)
                    {
                        MessageBox.Show("لا يوجد فواتير في هذا اليوم ");
                        return;
                    }
                    else
                    {

                        stack_visibie1.Visibility = Visibility.Visible;
                        stack_visibie.Visibility = Visibility.Hidden;
                        List<Invoice_Itemm> f = new List<Invoice_Itemm>();

                        for (int i = 0; i < check.Count; i++)
                        {
                            f.Add(new Invoice_Itemm { Invoice_id = check[i].id, Worker_name = check[i].worker_.name, Amount = check[i].amount, Discount = (double?)check[i].dicount, Created_at = check[i].created_at });
                            if (check[i].dicount == null) check[i].dicount = 0;
                            total += (check[i].amount - check[i].dicount);
                        }

                        list_x_fordate.ItemsSource = f;
                        total_invoice.Text = total.ToString();
                        
                    }


                }
                else
                {

                    MessageBox.Show("برحاء اختيار التاريخ ");
                    return;
                }
            }



        }

        private void come_back_button(object sender, RoutedEventArgs e)
        {

            Window2 ww = new Window2();
            ww.Show();
            this.Close();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string messageBoxText = "هل تريد حقا مسح العنصر ؟";
            string caption = "تحذير";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);


            if (result == MessageBoxResult.Yes)
            {


                int num = Convert.ToInt32(invoice_id.Text);
                var n = db.invoices.Where(x => x.id == num).First();
                n.deleted = true;
                db.invoices.AddOrUpdate(n);
                db.SaveChanges();
                MessageBox.Show("تم مسح العنصر بنجاح");
                stack_visibie.Visibility = Visibility.Hidden;
            }
        }
    }
}
