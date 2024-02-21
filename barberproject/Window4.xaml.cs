using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;

namespace barberproject
{
    /// <summary>
    /// Interaction logic for Window4.xaml
    /// </summary>
    public partial class Window4 : Window
    {
        barberEntities1 db = new barberEntities1();

        public Window4()
        {
            InitializeComponent();
        }
        public class Itemm
        {
            public int? id { get; set; }
            public string descriptio { get; set; }
            public int? price { get; set; }



        }
        public class calculate
        {
            public int? amount { get; set; }
            public int? sum { get; set; }

        }

        private void Window_Initialized(object sender, System.EventArgs e)
        {
            double a;
            List<Itemm> mmm = new List<Itemm>();
            var v = db.invoices.Max(b => b.id);
            var total = db.invoices.Where(b => b.id == v).ToList();
            if (total[0].dicount == null)
                a = (double)(total[0].amount);
            else a = (double)(total[0].amount - total[0].dicount);
            worker_name.Text = total[0].worker_.name;
            invoice_number.Text = total[0].id.ToString();
            date_time.Text = DateTime.Now.ToString();
            total_price.Text = a.ToString();
            List<invoice_items> n = db.invoice_items.Where(m => m.invoice_id == v).ToList();

            for (int i = 0; i < n.Count(); i++)
            {
                int c = n[i].item_id;
                var x = db.price_List.Where(f => f.ID == c).ToList();


                mmm.Add(new Itemm { id = x[0].ID, descriptio = x[0].description, price = x[0].Price });
                list_x.Items.Add(mmm[i]);
            }



        }
    }
}
