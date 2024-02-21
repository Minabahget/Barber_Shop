using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Diagnostics.Eventing.Reader;
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
    /// Interaction logic for salary_win.xaml
    /// </summary>
    public partial class salary_win : Window
    {
        barberEntities1 db = new barberEntities1();

        public class liitem
        {
            public int id { get; set; }
            public string name { get; set; }
            public TimeSpan in_time { get; set; }
            public TimeSpan? out_time { get; set; }
            public bool? paid { get; set; }
            public DateTime date { get; set; }
            public double? total_amount { get; set; }
            public double? deduction_in { get; set; }
            public double? deduction_out { get; set; }
            public double? bonus { get; set; }
        }
        public salary_win()
        {
            InitializeComponent();
            load_combo();
        }
        public void load_combo()
        {

            List<worker_> ii = db.worker_.Where(x => x.deleted == null).ToList();
            for (int iii = 0; iii < ii.Count; iii++)
            {
                combosearch.Items.Add(ii[iii].ID.ToString() + " " + ii[iii].name);
            }

        }

        private void combosalary_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            paid_btn.Visibility = Visibility.Hidden;
            string s = combosalary.SelectedItem.ToString();
            if (s.Contains(m2.Content.ToString()))
            {

                date_dock.Visibility = Visibility.Hidden;
                date_dock1.Visibility = Visibility.Visible;

                cal.Visibility = Visibility.Visible;
            }
            else if (s.Contains(m1.Content.ToString()))
            {
                date_dock1.Visibility = Visibility.Hidden;
                date_dock.Visibility = Visibility.Visible;
                cal.Visibility = Visibility.Visible;

            }

        }

        private void combosearch_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            paid_btn.Visibility = Visibility.Hidden;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window2 w = new Window2();
            w.Show();
            this.Close();
        }



        public void calculate(object sender, RoutedEventArgs e)
        {
            paid_btn.Visibility = Visibility.Visible;
            list.Items.Clear();
            if (combosalary.SelectedItem == null) { MessageBox.Show("برجاء اختيار طريقة القبض "); return; }
            string selected_item = combosalary.SelectedItem.ToString();
            if (selected_item.Contains(m1.Content.ToString()))
            {
                if (date_text.Text == "")
                {
                    MessageBox.Show("برجاء ادخال تاريخ القبض");
                    return;
                }
                dock_total.Visibility = Visibility.Hidden;
            }
            if (selected_item.Contains(m2.Content.ToString()))
            {
                if (date_text1.Text == "")
                {
                    MessageBox.Show("برجاء ادخال نهاية  تاريخ القبض الاسبوعي");
                    return;
                }
                dock_total.Visibility = Visibility.Visible;

            }
            paid_btn.Visibility = Visibility.Visible;
            list_stack.Visibility = Visibility.Visible;

            // يومي 

            if (selected_item == combosalary.Items[0].ToString())
            {
                // take the date 

                int day = date_text.SelectedDate.Value.Day;
                int month = date_text.SelectedDate.Value.Month;
                int year = date_text.SelectedDate.Value.Year;
                DateTime date = new DateTime(year, month, day);
                if (combosearch.SelectedItem == null)
                {
                    List<liitem> item = new List<liitem>();
                    workers_salary Salary = new workers_salary();
                    var check = db.workers_salary.Where(z => z.date == date).ToList();
                    if (check.Count != 0)
                    {
                        for (int i = 0; i < check.Count; i++)
                        {
                            int id = check[i].worker_id;
                            var check1 = db.worker_.Where(c => c.ID == id).ToList();
                            if (check1.Count == 0)
                            {
                                continue;
                            }

                            item.Add(new liitem()
                            {
                                id = check[i].worker_id,
                                name = check1[0].name,
                                in_time = check[i].in_time,
                                out_time = check[i].out_time,
                                deduction_in = check[i].deduction_in,
                                deduction_out = check[i].deduction_out,
                                date = check[i].date,
                                bonus = check[i].Bonus,
                                total_amount = check[i].total_salary,
                                paid = check[i].Paid
                            });
                            list.Items.Add(item);

                        }





                    }
                    else { MessageBox.Show("لا يوجد عاملين في هذا اليوم "); return; }
                }
                else if (combosearch.SelectedItem != null)
                {
                    string select = combosearch.SelectedItem.ToString();
                    string[] selc = select.Split(' ');
                    int id = Convert.ToInt32(selc[0]);
                    string name = selc[1];
                    var get = db.workers_salary.Where(s => s.worker_id == id && s.date == date).ToList();
                    if (get.Count == 0)
                    {
                        MessageBox.Show("لا يوجد قبض لهذا العامل اليوم ");
                        return;
                    }
                    else if (get.Count != 0)
                    {
                        List<liitem> itemm = new List<liitem>();

                        itemm.Add(new liitem()
                        {
                            id = get[0].worker_id,
                            name = name,
                            in_time = get[0].in_time,
                            out_time = get[0].out_time,
                            deduction_in = get[0].deduction_in,
                            deduction_out = get[0].deduction_out,
                            date = get[0].date,
                            bonus = get[0].Bonus,
                            total_amount = get[0].total_salary,
                            paid = get[0].Paid
                        });
                        list.Items.Add(itemm);

                    }




                }



            }
            //اسبوعي 
            if (selected_item == combosalary.Items[1].ToString())
            {
                double? total_salary_s = 0;
                workers_salary Salary1 = new workers_salary();
                int day = date_text1.SelectedDate.Value.Day;
                int month = date_text1.SelectedDate.Value.Month;
                int year = date_text1.SelectedDate.Value.Year;
                DateTime date = new DateTime(year, month, day);
                if (combosearch.SelectedItem == null)
                {
                    MessageBox.Show("برجاء اختيار اسم العامل ");
                    return;
                }
                string select = combosearch.SelectedItem.ToString();
                string[] selc = select.Split(' ');
                int id = Convert.ToInt32(selc[0]);
                string name = selc[1];


                for (int i = 0; i < 7; i++)
                {
                    List<liitem> item1 = new List<liitem>();

                    var date1 = date.AddDays(-i);
                    var Week_salary = db.workers_salary.Where(z => z.worker_id == id && z.date == date1).FirstOrDefault();
                    if (Week_salary != null)
                    {

                        item1.Add(new liitem()
                        {
                            id = Week_salary.worker_id,
                            name = name,
                            in_time = Week_salary.in_time,
                            out_time = Week_salary.out_time,
                            deduction_in = Week_salary.deduction_in,
                            deduction_out = Week_salary.deduction_out,
                            date = Week_salary.date,
                            bonus = Week_salary.Bonus,
                            total_amount = Week_salary.total_salary,
                            paid = Week_salary.Paid
                        });

                    }
                    if (Week_salary == null)
                    {
                        item1.Add(new liitem()
                        {
                            id = id,
                            name = name,
                            in_time = default,
                            out_time = default,
                            deduction_in = 0,
                            deduction_out = 0,
                            date = date1,
                            bonus = 0,
                            total_amount = 0,
                            paid = true
                        });

                    }

                    list.Items.Add(item1);
                    if (item1[0].paid == false)
                        total_salary_s += item1[0].total_amount + item1[0].bonus;

                }
                total_t.Text = total_salary_s.ToString();
            }


        }

        private void paid_btn_Click(object sender, RoutedEventArgs e)
        {
            // weekly 

            string s = combosalary.SelectedItem.ToString();
            if (s.Contains(m2.Content.ToString()))
            {
                string select = combosearch.SelectedItem.ToString();
                string[] selc = select.Split(' ');

                int wokrer_id = Convert.ToInt32(selc[0]);
                int day = date_text1.SelectedDate.Value.Day;
                int month = date_text1.SelectedDate.Value.Month;
                int year = date_text1.SelectedDate.Value.Year;
                DateTime date = new DateTime(year, month, day);
                for (int i = 0; i < 7; i++)
                {
                    var date_update = date.AddDays(-i);
                    workers_salary w = new workers_salary();
                    var check = db.workers_salary.Where(a => a.worker_id == wokrer_id && a.date == date_update).ToList();
                    if (check.Count != 0)
                    {
                        check[0].Paid = true;
                        db.workers_salary.AddOrUpdate(check[0]);
                        db.SaveChanges();
                    }

                }


            }



            else if (s.Contains(m1.Content.ToString()))
            {
                // daily one (no name checked)
                int day = date_text.SelectedDate.Value.Day;
                int month = date_text.SelectedDate.Value.Month;
                int year = date_text.SelectedDate.Value.Year;
                DateTime date = new DateTime(year, month, day);

                if (combosearch.SelectedIndex == -1)
                {

                    var get = db.workers_salary.Where(h => h.date == date).ToList();
                    for (int i = 0; i < get.Count; i++)
                    {
                        get[i].Paid = true;
                        db.workers_salary.AddOrUpdate(get[i]);

                    }


                    db.SaveChanges();
                }


                // daily all (name checked)
                else
                {
                    string select = combosearch.SelectedItem.ToString();
                    string[] selc = select.Split(' ');

                    int wokrer_id = Convert.ToInt32(selc[0]);
                    var get = db.workers_salary.Where(h => h.date == date && h.id == wokrer_id).ToList();
                    if (get.Count != 0)
                    {
                        get[0].Paid = true;
                        db.workers_salary.AddOrUpdate(get[0]);

                        db.SaveChanges();
                    }




                }
            }



            MessageBox.Show("تم القبض بنجاح ");



        }

        private void date_text1_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            paid_btn.Visibility = Visibility.Hidden;
        }

        private void date_text_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            paid_btn.Visibility = Visibility.Hidden;
        }
    }
}
