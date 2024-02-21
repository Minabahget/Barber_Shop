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
using System.Security;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.RightsManagement;
using System.Windows.Interop;

namespace barberproject
{

    /// <summary>
    /// Interaction logic for addw.xaml
    /// </summary>
    /// 

    public partial class addw : Window
    {
        barberEntities1 db = new barberEntities1();
        public addw()
        {
            try
            {
                InitializeComponent();
                GetAllUserInfo();
                load_datagrid();
                load_combobox();
            }
            catch (Exception e){ 
            MessageBox.Show(e.Message);
            
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Window2 window = new Window2();
            window.Show();
            this.Close();
        }

        public class iitem
        {
            public string name { get; set; }
            public int idd { get; set; }
            public int daily_salary { get; set; }

            public double? bonus_rate { get; set; }

            public TimeSpan? start_time_gird { get; set; }

            public TimeSpan? end_time_gird { get; set; }




        }
        public void load_combobox()

        {
            List<worker_> ii = db.worker_.Where(x => x.deleted == null).ToList();


            for (int iii = 0; iii < ii.Count; iii++)
            {



                combo_periss.Items.Add(ii[iii].ID + " " + ii[iii].name);

            }


        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sIp"></param>
        /// <param name="iPort"></param>
        /// <param name="iMachineNumber"></param>
        public void GetAllUserInfo(string sIp = "192.168.1.201", int iPort = 4370, int iMachineNumber = 1)
        {
            //Create Standalone SDK class dynamicly.
            zkemkeeper.CZKEMClass axCZKEM1 = new zkemkeeper.CZKEMClass();

            axCZKEM1.Connect_Net(sIp, iPort);
            string iEnrollNumber = " ";
            string sname = String.Empty;
            string sPass = String.Empty;
            int iPrivilege = 0;
            bool ienabled = false;

            axCZKEM1.EnableDevice(iMachineNumber, false);
            axCZKEM1.ReadAllUserID(iMachineNumber);//read all the user information to the memory
            while (axCZKEM1.SSR_GetAllUserInfo(iMachineNumber, out iEnrollNumber, out sname, out sPass, out iPrivilege, out ienabled))
            {
                combo_data.Items.Add(iEnrollNumber + " " + sname);
            }


            axCZKEM1.EnableDevice(iMachineNumber, true);
        }

        private void add_button_Click(object sender, RoutedEventArgs e)
        {


            if (combo_data.SelectedItem ==null)
            {
                MessageBox.Show("من فضلك اختر اسم ورقم العامل");
                return;
            }
            string sName = combo_data.SelectedItem.ToString();
            var x = sName.Split(' ');
            int id = Convert.ToInt32(x[0]);
            var name = x[1];

            if (combo_start.SelectedIndex == -1)
            {

                MessageBox.Show("برجاء ادخال بداية الشيفت   ");
                return;
            }

            if (salarytxt.Text == "")
            {
                MessageBox.Show("برجاء ادخال المرتب ");
                return;
            }
            if (bonus.Text == "")
            {
                MessageBox.Show("برجاء ادخال نسبة الارباح ");
                return;
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(salarytxt.Text, "[^0-9]"))
            {
                MessageBox.Show("برجاء ادخال المرتب بصيغة صحيحة ");
                salarytxt.Text = string.Empty;
                return;
            }
            if (System.Text.RegularExpressions.Regex.IsMatch(bonus.Text, "[^0-9]"))
            {
                MessageBox.Show("برجاء ادخال نسبة الارباح بصيغة صحيحة ");
                salarytxt.Text = string.Empty;
                return;
            }

            if (!(Convert.ToDouble(bonus.Text) >= 1 && Convert.ToDouble(bonus.Text) <= 100))
            {
                MessageBox.Show("برجاء ادخال نسبة الارباح برقم صحيح ");
                salarytxt.Text = string.Empty;
                return;
            }
            if (combo_data.SelectedItem == null)
            {
                MessageBox.Show("برجاء اختيار الموظف ");
                return;

            }
            int salary = Convert.ToInt32(salarytxt.Text);
            worker_ w = new worker_();
            w.ID = id;
            w.name = name;
            w.daily_salary = salary;

            double bonus_number = Convert.ToDouble(bonus.Text) / 100.0;
            w.Bonus_Rate = bonus_number;

            string cv = combo_start.SelectedItem.ToString();

            string[] cv_list = cv.Split(':');

            TimeSpan s = new TimeSpan(Convert.ToInt32(cv_list[1]), Convert.ToInt32(cv_list[2]), 0);
            w.Start_Time = s;

            w.End_Time = new TimeSpan((Convert.ToInt32(cv_list[1]) + 12) < 24 ?
                                        (Convert.ToInt32(cv_list[1]) + 12) : (Convert.ToInt32(cv_list[1]) + 12) - 24
                , Convert.ToInt32(cv_list[2]), 0);

            var check = db.worker_.Where(c => c.ID == id && c.daily_salary == salary && c.name == name && c.Start_Time == s && c.Bonus_Rate == bonus_number && c.deleted == null).ToList();
            if (check.Count > 0)
            {
                MessageBox.Show("تم التعديل بالفعل برجاء عدم المحاولة ");
                return;
            }
            db.worker_.AddOrUpdate(w);


            db.SaveChanges();
            MessageBox.Show("تم حفظ التغيرات ");
            combo_data.SelectedItem = " ";
            salarytxt.Text = "";
            bonus.Text = "";
            combo_start.Text = string.Empty;


        }





        private void combo_data_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string sName = combo_data.SelectedItem.ToString();
            var x = sName.Split(' ');
            var id = Convert.ToInt32(x[0]);
            var name = x[1];
            var m = db.worker_.Where(l => l.ID == id && l.name == name && l.deleted == null).ToList();
            if (m.Count == 0)
            {


                System.Windows.Forms.MessageBox.Show("برجاء اضافة بيانات الموظف   ");
                return;


            }
            else
            {
                salarytxt.Text = m[0].daily_salary.ToString();
                combo_start.Text = m[0].Start_Time.ToString();
                bonus.Text = (m[0].Bonus_Rate * 100.0).ToString();

            }


        }
        public void load_datagrid()
        {

            List<iitem> i = new List<iitem>();
            List<worker_> ii = db.worker_.Where(x => x.deleted == null).ToList();



            for (int iii = 0; iii < ii.Count; iii++)
            {
                i.Add(new iitem { idd = ii[iii].ID, name = ii[iii].name, daily_salary = ii[iii].daily_salary, bonus_rate = ii[iii].Bonus_Rate * 100.0, start_time_gird = ii[iii].Start_Time, end_time_gird = ii[iii].End_Time });


            }

            datagrid_worker.ItemsSource = i;



        }


        private void load_Click(object sender, RoutedEventArgs e)
        {
            load_datagrid();
            MessageBox.Show("تم اعادة التحميل ");
        }

        private void btn_delete_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            string messageBoxText = "هل تريد حقا مسح العنصر ؟";
            string caption = "تحذير";
            MessageBoxButton button = MessageBoxButton.YesNoCancel;
            MessageBoxImage icon = MessageBoxImage.Warning;
            MessageBoxResult result;

            result = System.Windows.MessageBox.Show(messageBoxText, caption, button, icon, MessageBoxResult.Yes);


            if (result == MessageBoxResult.Yes)
            {


                iitem i = new iitem();
                i = datagrid_worker.CurrentItem as iitem;

                var n = db.worker_.Where(x => x.ID == i.idd).First();
                n.deleted = true;
                db.worker_.AddOrUpdate(n);
                db.SaveChanges();
                MessageBox.Show("تم مسح العنصر بنجاح");
                load_datagrid();
            }

        }

        private void add_button_Click2(object sender, RoutedEventArgs e)
        {
            permission per = new permission();
            permission per1 = new permission();
            if (combo_periss.SelectedItem == null)
            {
                MessageBox.Show("من فضلك اختر اسم ورقم العامل");
                return;
            }
            if (permiss1.IsChecked == false && permi2.IsChecked == false)
            {
                MessageBox.Show("برجاء اختيار نوع الاستئذان");
                return;
            }
            if (permiss1.IsChecked == true && permi2.IsChecked == true)
            {
                MessageBox.Show("برجاء اختيار نوع استئذان واحد");
                return;
            }
            if (permiss1.IsChecked == true)
            {
                if (permission_date.SelectedDate == null)
                {
                    MessageBox.Show("من فضلك اختر تاريخ الاستئذان");
                    return;
                }
                string sName = combo_periss.SelectedItem.ToString();
                var x = sName.Split(' ');
                var id = Convert.ToInt32(x[0]);
                var name = x[1];
                var t = permission_date.SelectedDate.Value.ToString("yyyy-MM-dd");



                var check = db.permissions.Where(d => d.worker_ID == id && d.date_permission.ToString().Contains(t) && d.persmission == 1).ToList();
                if (check.Count != 0)
                {
                    MessageBox.Show("تم الاستئذان بالفعل");
                    return;
                }



                var xx = t.Split('-');
                int year = Convert.ToInt32(xx[0]);
                int month = Convert.ToInt32(xx[1]);
                int day = Convert.ToInt32(xx[2]);
                DateTime date = new DateTime(year, month, day);
                per.worker_ID = id;
                per.persmission = 2;
                per.date_permission = date;
                db.permissions.AddOrUpdate(per);
                db.SaveChanges();
                MessageBox.Show("تم حفظ الاستئذان");

            }
            if (permi2.IsChecked == true)
            {


                if (permission2_date.SelectedDate == null)
                {
                    MessageBox.Show("من فضلك اختر تاريخ الاستئذان");
                    return;
                }
                string sName = combo_periss.SelectedItem.ToString();
                var x = sName.Split(' ');
                var id = Convert.ToInt32(x[0]);
                var name = x[1];
                var t = permission2_date.SelectedDate.Value.ToString("yyyy-MM-dd");
                var check = db.permissions.Where(d => d.worker_ID == id && d.date_permission.ToString().Contains(t) && d.persmission == 2).ToList();
                if (check.Count != 0)
                {
                    MessageBox.Show("تم الاستئذان بالفعل");
                    return;
                }
                var xxx = t.Split('-');
                int year = Convert.ToInt32(xxx[0]);
                int month = Convert.ToInt32(xxx[1]);
                int day = Convert.ToInt32(xxx[2]);
                DateTime date = new DateTime(year, month, day);
                per1.worker_ID = id;
                per1.persmission = 1;
                per1.date_permission = date;
                db.permissions.Add(per1);
                db.SaveChanges();
                MessageBox.Show("تم حفظ الاستئذان");


            }


        }
    }
}
