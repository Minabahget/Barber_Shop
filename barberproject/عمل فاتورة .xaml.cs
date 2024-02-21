using System;
using System.Collections.Generic;
using System.Linq;
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

namespace barberproject
{
    /// <summary>
    /// Interaction logic for عمل_فاتورة.xaml
    /// </summary>
    public partial class make_invoice : Window
    {
        barberEntities1 db = new barberEntities1();
        int count = 0;
        public make_invoice()
        {
            InitializeComponent();
            load_combobox();
        }
        private void DataGrid_Initialized(object sender, EventArgs e)
        {
            List<liitem> f = new List<liitem>();
            List<price_List> ii = db.price_List.Where(x => x.deleted == null).ToList();

            for (int iii = 0; iii < ii.Count; iii++)
            {
                f.Add(new liitem { id = ii[iii].ID.ToString(), describtion = ii[iii].description, price = ii[iii].Price.ToString() });
            }

            datagrid.ItemsSource = f;

        }

        public void load_combobox()

        {
            List<worker_> ii = db.worker_.Where(x => x.deleted == null).ToList();
            for (int iii = 0; iii < ii.Count; iii++)
            {
                idcombo.Items.Add(ii[iii].name + "     " + ii[iii].ID);
            }

        }


        public class liitem
        {
            public string describtion { get; set; }
            public string id { get; set; }
            public string price { get; set; }



        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            List<liitem> i = new List<liitem>();
            liitem pr = datagrid.SelectedItem as liitem;
            i.Add(new liitem() { id = pr.id.ToString(), describtion = pr.describtion, price = pr.price.ToString() });
            count += int.Parse(pr.price);
            amount.Text = count.ToString();
            list.Items.Add(i);
            invoice_items item = new invoice_items();
            item.item_id = Int32.Parse(i[0].id);

            db.invoice_items.Add(item);
        }


        private void come_back_button(object sender, RoutedEventArgs e)
        {

            Window2 w = new Window2();
            w.Show();
            this.Close();
        }
        public bool IsValidintFormat(string input)
        {
            int dummyOutput;
            return int.TryParse(input, out dummyOutput);
        }




        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            if (idcombo.SelectedItem == null)
            {
                MessageBox.Show("من فضلك اختر اسم ورقم العامل");
                return;
            }

            if (list.Items.Count == 0)
            {
                MessageBox.Show("من فضلك ااكمل عناصر الفاتورة  ");
                return;
            }
            if (discount1.Text != "")
            {
                if (System.Text.RegularExpressions.Regex.IsMatch(discount1.Text, "[^0-9]"))
                {
                    MessageBox.Show("برجاء ادخال الخصم بصيغة صحيحة ");
                    discount1.Text = string.Empty;
                    return;
                }
                var n = true;
                n = IsValidintFormat(discount1.Text);
                int num = int.Parse(discount1.Text);
                if (n == true)
                {

                    if (num < 0)
                    {
                        MessageBox.Show("برجاء ادخال رقم موجب");
                        return;
                    }

                }
                if (n == false)
                {

                    MessageBox.Show("برجاء ادخال الخصم بصيغة صحيحة ");
                    return;
                }

            }

            if ((discount1.Text == "" ? 0 : int.Parse(discount1.Text)) > int.Parse(amount.Text))
            {
                MessageBox.Show("لا يمكن  قيمة الخصم اكبر من اجمالي الفاتورة");
                return;
            }


            string select = idcombo.SelectedItem.ToString();
            string[] selc = select.Split(' ');
            string select1 = selc[0];
            worker_ d = db.worker_.Where(x => x.name == select1 && x.deleted == null).FirstOrDefault();
            List<liitem> m = new List<liitem>();
            liitem r = datagrid.SelectedItem as liitem;
            for (int i = 0; i < list.Items.Count; i++)
            {
                m.Add(new liitem { id = r.id.ToString(), price = r.price.ToString(), describtion = r.describtion });
            }
            invoice v = new invoice();
            if (discount1.Text == "")
            {
                v.worker_id = d.ID;
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 7) v.created_at = DateTime.Now.AddDays(-1);
                else v.created_at = DateTime.Now;
                v.amount = float.Parse(amount.Text);
                db.invoices.Add(v);
                db.SaveChanges();
                MessageBox.Show("تمت ");


            }
            else
            {

                v.dicount = float.Parse(discount1.Text);
                v.worker_id = d.ID;
                if (DateTime.Now.Hour >= 0 && DateTime.Now.Hour <= 7) v.created_at = DateTime.Now.AddDays(-1);
                else v.created_at = DateTime.Now;
                v.amount = float.Parse(amount.Text);
                db.invoices.Add(v);
                db.SaveChanges();
                MessageBox.Show("تمت بنجاح");

            }

            discount1.Text = "";
            amount.Text = "";
            list.Items.Clear();
            count = 0;
            Window4 w = new Window4();
            w.Show();
            PrintDialog p = new PrintDialog();
            p.PrintVisual(w, "print window");
            w.Close();
        }


    }
}
