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
    /// Interaction logic for password_win.xaml
    /// </summary>
    public partial class password_win : Window
    {
        public password_win()
        {
            InitializeComponent();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (pass_text.Password == "4444441")

            {
                App.Current.Windows[0].Close();
                addw w = new addw();
                w.Show();
                this.Close();
                return;

            }
            else if (pass_text.Password == "4444442")

            {
                App.Current.Windows[0].Close();
                salary_win w = new salary_win();
                w.Show();
                this.Close();
                return;

            }

            else if (pass_text.Password == "")
            {
                MessageBox.Show("برجاء ادخال كلمة المرور");
                return;

            }
            else
            {
                MessageBox.Show("كلمة مرور خاطئة");
                pass_text.Password = null;
                return;
            }
        }


    }

}
