using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Permissions;
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
    /// Interaction logic for date.xaml
    /// </summary>
    public partial class date : Window
    {
        public date()
        {
            InitializeComponent();


        }

        public void save_data()
        {
            barberEntities1 db = new barberEntities1();
            string dbname = db.Database.Connection.Database;
            string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'MyAir-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";
            db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname, "Amin9999999999999"));
        }



        //void LoadDB( System.Data.Entity.DbContext context,
        // string backup_filename,
        // string orig_mdf, // the original LogicalName name of the data (also called the MDF) file within the backup file
        // string orig_ldf, // the original LogicalName name of the log (also called the LDF) file within the backup file
        // string new_database_name)
        //     {
        //         var database_dir = System.IO.Path.GetTempPath();
        //         var temp_mdf = $"{database_dir}{new_database_name}.mdf";
        //         var temp_ldf = $"{database_dir}{new_database_name}.ldf";
        //         var query = @"USE master; RESTORE DATABASE @new_database_name FROM DISK = @backup_filename
        //     WITH MOVE @orig_mdf TO @temp_mdf,
        //     MOVE @orig_ldf TO  @temp_ldf,
        //     REPLACE;";
        //         context.Database.ExecuteSqlCommand(
        //             // Do not use a transaction for this query so we can load without getting an exception:
        //             // "cannot perform a backup or restore operation within a transaction"
        //             TransactionalBehavior.DoNotEnsureTransaction,
        //             query,
        //             new[] {
        //     new SqlParameter("@backup_filename", backup_filename),
        //     new SqlParameter("@database_dir", database_dir),
        //     new SqlParameter("@new_database_name", new_database_name),
        //     new SqlParameter("@orig_mdf", orig_mdf),
        //     new SqlParameter("@orig_ldf", orig_ldf),
        //     new SqlParameter("@temp_mdf", temp_mdf),
        //     new SqlParameter("@temp_ldf", temp_ldf),
        //             }
        //         );
        //     }


        public void Store_Culculate_Data(string date, string date1)
        {
            barberEntities1 barberEntities1 = new barberEntities1();
            var x = date.Split('-');
            int year = Convert.ToInt32(x[0]);
            int month = Convert.ToInt32(x[1]);
            int day = Convert.ToInt32(x[2]);
            DateTime day_date = new DateTime(year, month, day);
            Device device_Data = new Device();
            var check = barberEntities1.Devices.Where(d => d.date.ToString().Contains(date)).ToList();
            var check1 = barberEntities1.Devices.Where(f => f.date.ToString().Contains(date1) && f.Time.Hours > 0 && f.Time.Hours < 7).ToList();
            check.AddRange(check1);
            for (int i = 0; i < check.Count; i++)
            {

                workers_salary workers_Salary = new workers_salary();
                int id = check[i].worker_id;
                var time = check[i].Time;

                if (check[i].mode == 0)
                {
                    var check_Start = barberEntities1.worker_.Where(a => a.ID == id && a.Start_Time <= time).ToList();

                    var check_In = barberEntities1.workers_salary.Where(v => v.worker_id == id && v.date.ToString().Contains(date)).ToList();
                    if (check_Start.Count != 0 && check_In.Count == 0)
                    {
                        workers_Salary.in_time = check[i].Time;
                        workers_Salary.date = day_date;
                        workers_Salary.worker_id = check[i].worker_id;
                        var check_perm_in = barberEntities1.permissions.Where(w => w.worker_ID == id && w.date_permission.ToString().Contains(date) && w.persmission == 1).ToList();
                        if (check_perm_in.Count == 0)
                        {
                           
                            // calculate the exact minutes after the 30 minutes permission 

                            workers_Salary.permission__in = false;
                            TimeSpan difference = check[i].Time.Subtract(check_Start[0].Start_Time.Value);
                            if(difference.TotalMinutes>0&&difference.TotalMinutes<=30)
                            {
                                workers_Salary.cal_min = Math.Round(((check_Start[i].daily_salary / 12.0) / 60.0) * (difference.TotalMinutes), 2);


                            }
                            if (difference.TotalMinutes > 30)
                            {
                                workers_Salary.deduction_in = Math.Round(((check_Start[0].daily_salary / 12.0) / 60.0) * (difference.TotalMinutes * 2.0), 2);
                            }

                            else workers_Salary.deduction_in = 0;
                        }

                        else
                        {
                            workers_Salary.deduction_in = 0;
                            workers_Salary.permission__in = true;

                        }


                        barberEntities1.workers_salary.Add(workers_Salary);
                        barberEntities1.SaveChanges();
                    }
                    else
                        continue;
                }
                if (check[i].mode == 1)
                {
                    var find = barberEntities1.worker_.Where(m => m.ID == id).FirstOrDefault();
                    var check_out = barberEntities1.workers_salary.Where(c => c.worker_id == id && c.out_time == null && c.in_time != null && c.date == day_date).ToList();

                    if (check_out.Count != 0)
                    {
                        check_out[0].out_time = check[i].Time;
                        double working_minutes = ((time - find.Start_Time.Value).TotalMinutes) < 0 ?
                           ((time - find.Start_Time.Value).TotalMinutes) + (24 * 60) : ((time - find.Start_Time.Value).TotalMinutes);

                        double working_hours = ((time - find.Start_Time.Value).TotalHours) < 0 ?
                            ((time - find.Start_Time.Value).TotalHours) + 24 : ((time - find.Start_Time.Value).TotalHours);

                        double deduction_out = 0.0;

                        double working_salary = working_minutes * ((find.daily_salary / 12.0) / 60.0);

                        var check_perout_out = barberEntities1.permissions.Where(w => w.worker_ID == id && w.date_permission == day_date && w.persmission == 2).ToList();

                        if (working_hours < 12 && check_perout_out.Count == 0)
                        {
                            check_out[0].permission__out = false;
                            double deduction_out_minutes = ((find.End_Time.Value - time).TotalMinutes) < 0 ?
                                ((find.End_Time.Value - time).TotalMinutes) + (24 * 60) : ((find.End_Time.Value - time).TotalMinutes);
                            deduction_out = (deduction_out_minutes * 1.0) * ((find.daily_salary / 12.0) / 60.0) * 2.0;
                            if (working_salary < 0) working_salary = 0;
                        }

                        if (check_perout_out.Count != 0) check_out[0].permission__out = true;
                        working_salary = Math.Round(working_salary, 2);
                        if (check_out[0].cal_min == null) check_out[0].cal_min = 0;
                        var total = working_salary - check_out[0].deduction_in - deduction_out - check_out[0].cal_min;

                        total = Math.Round((double)total, 2);
                        if (total < 0) total = 0;

                        check_out[0].deduction_out = deduction_out;
                        check_out[0].working_minutes = Convert.ToInt32(working_minutes);

                        check_out[0].total_salary = total;

                        //bonus 
                        var check6 = barberEntities1.invoices.Where(r => r.worker_id == id && r.created_at.Year == year && r.created_at.Month == month && r.created_at.Day == day && r.deleted == null).ToList();
                        double? bonus = 0, total_bills = 0, discount = 0;

                        if (check6.Count != 0)
                        {
                            foreach (var item in check6)
                            {
                                if (item.dicount == null) discount = 0;
                                else discount = item.dicount;

                                total_bills += (item.amount - discount);

                            }

                            bonus = check6[0].worker_.Bonus_Rate * total_bills;
                        }

                        check_out[0].Bonus = bonus;
                        barberEntities1.workers_salary.AddOrUpdate(check_out[0]);
                        barberEntities1.SaveChanges();
                    }
                    else
                        continue;

                }



            }
        }
        public void Get_Data(string sIpp = "192.168.1.201", int iPortt = 4370)
        {
            zkemkeeper.CZKEMClass axCZKEM2 = new zkemkeeper.CZKEMClass();

            int iMachineNumberr = 1;
            axCZKEM2.Connect_Net(sIpp, iPortt);
            string sdwEnrollNumberr = "";
            int idwVerifyModee = 0;
            int idwInOutModee = 0;
            int idwYearr = 0;
            int idwMonthh = 0;
            int idwDayy = 0;
            int idwHourr = 0;
            int idwMinutee = 0;
            int idwSecondd = 0;
            int idwWorkcodee = 0;


            barberEntities1 database2 = new barberEntities1();



            axCZKEM2.EnableDevice(iMachineNumberr, false);//disable the device
            if (axCZKEM2.ReadGeneralLogData(iMachineNumberr))//read all the attendance records to the memory
            {
                database2.Devices.RemoveRange(database2.Devices.ToList());
                while (axCZKEM2.SSR_GetGeneralLogData(iMachineNumberr, out sdwEnrollNumberr, out idwVerifyModee,
                            out idwInOutModee, out idwYearr, out idwMonthh, out idwDayy, out idwHourr, out idwMinutee, out idwSecondd, ref idwWorkcodee))//get records from the memory
                {
                    TimeSpan time = new TimeSpan(idwHourr, idwMinutee, idwSecondd);
                    int id = Convert.ToInt32(sdwEnrollNumberr);
                    DateTime date = new DateTime(idwYearr, idwMonthh, idwDayy);
                    Device device_Data = new Device();
                    device_Data.worker_id = id;
                    device_Data.date = date;
                    device_Data.Time = time;
                    device_Data.mode = idwInOutModee;
                    database2.Devices.Add(device_Data);
                    database2.SaveChanges();
                }
            }
        }

        private void worker_btn_Click(object sender, RoutedEventArgs e)
        {
            if (date_text.SelectedDate == null)
            {
                MessageBox.Show("برجاء ادخال التاريخ ");
                return;
            }

            var date = date_text.SelectedDate.Value.ToString("yyyy-MM-dd");
            var date1 = date_text.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd");
            Get_Data();
            Store_Culculate_Data(date, date1);
            MessageBox.Show("تم حفظ البيانات ");
            // save_data();
            barberEntities1 db = new barberEntities1();
            //  LoadDB(db, "Amin9999999999999", "barber ", "barber _log", "barber ");
            this.Close();





        }
    }
}
