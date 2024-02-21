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
using System.Data.Entity.Migrations;

namespace barberproject
{
    /// <summary>
    /// Interaction logic for inventory_Win.xaml
    /// </summary>
    public partial class inventory_Win : Window
    {
        barberEntities1 db = new barberEntities1();
        public inventory_Win()
        {
            InitializeComponent();
            load_datagrid();
        }

        public class iitem
        {
            public int id { get; set; }
            public string description { get; set; }
            public int count { get; set; }


        }
        public void load_datagrid()
        {

            List<iitem> i = new List<iitem>();
            List<inventory> ii = db.inventories.Where(x => x.deleted == null).ToList();
            for (int iii = 0; iii < ii.Count; iii++)
            {
                i.Add(new iitem { description = ii[iii].description, count = ii[iii].count, id = ii[iii].ID });
            }

            datagrid2.ItemsSource = i;
        }

        private void btn_delete_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string messageBoxText = "هل تريد حقا مسح العنصر ؟";
            string caption = "تحذير";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);
            if (result == MessageBoxResult.Yes)
            {
                iitem i = new iitem();
                i = datagrid2.CurrentItem as iitem;
                var n = db.inventories.Where(x => x.ID == i.id).First();
                n.deleted = true;
                db.inventories.AddOrUpdate(n);
                db.SaveChanges();
                MessageBox.Show("تم مسح العنصر بنجاح");
                load_datagrid();
            }

        }

        private void btn_1_Click(object sender, RoutedEventArgs e)
        {
            edit.Visibility = Visibility.Visible;
            add.Visibility = Visibility.Hidden;
            iitem i = new iitem();
            i = datagrid2.CurrentItem as iitem;

            inventory w = new inventory();
            w = db.inventories.Where(x => x.ID == i.id).FirstOrDefault();
            edit_id.Text = i.id.ToString();
            pricetex.Text = i.count.ToString();
            des_tex.Text = i.description.ToString();
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pricetex.Text) && string.IsNullOrEmpty(des_tex.Text))
            {
                {
                    MessageBox.Show(" برجاء ادخال بيانات العنصر الجديد  ");
                    return;
                }
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(pricetex.Text, "[^0-9]"))
            {
                MessageBox.Show("برجاء ادخال الكمية بطريقة صحيحة");
                pricetex.Text = pricetex.Text.Remove(pricetex.Text.Length - 1);
                pricetex.Text = " ";
                return;
            }
            if (pricetex.Text == "")
            {
                MessageBox.Show("برجاء ادخال الكمية ");
                return;

            }

            if (des_tex.Text == "")
            {
                MessageBox.Show("برجاء ادخال الوصف ");
                return;

            }
            int x = int.Parse(pricetex.Text);
            inventory i = new inventory();
            i.ID = int.Parse(edit_id.Text);
            i.description = des_tex.Text;
            i.count = x;
            db.inventories.AddOrUpdate(i);
            db.SaveChanges();
            MessageBox.Show("تم تعديل العنصر بنجاح");
            load_datagrid();
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(pricetex.Text) && string.IsNullOrEmpty(des_tex.Text))
            {
                {
                    MessageBox.Show(" برجاء ادخال بيانات العنصر الجديد  ");
                    return;
                }
            }

            if (System.Text.RegularExpressions.Regex.IsMatch(pricetex.Text, "[^0-9]"))
            {
                MessageBox.Show("برجاء ادخال الكمية بطريقة صحيحة");
                pricetex.Text = pricetex.Text.Remove(pricetex.Text.Length - 1);
                pricetex.Text = " ";
                return;
            }
            if (pricetex.Text == "")
            {
                MessageBox.Show("برجاء ادخال الكمية ");
                return;

            }

            if (des_tex.Text == "")
            {
                MessageBox.Show("برجاء ادخال الوصف ");
                return;

            }
            var m = db.inventories.Where(n => n.description == des_tex.Text).FirstOrDefault();
            if (m != null)
            {
                MessageBox.Show("الاسم موجود بالفعل برجاء تغيير الاسم");
                return;


            }
            int x = int.Parse(pricetex.Text);
            inventory i = new inventory();
            i.description = des_tex.Text;
            i.count = x;
            db.inventories.Add(i);
            db.SaveChanges();
            MessageBox.Show("تم اضافة العنصر بنجاح");
            load_datagrid();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            Window2 ww = new Window2();
            ww.Show();
            this.Close();
        }
    }
}
