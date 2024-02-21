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
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {

        barberEntities1 db = new barberEntities1();
        public Window1()
        {

            InitializeComponent();


        }




        private void userchangedd(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textuserrr.Text) && txtuserrr.Text.Length > 0)
                textuserrr.Visibility = Visibility.Collapsed;
            else
                textuserrr.Visibility = Visibility.Visible;
        }

        private void usermousedownn(object sender, MouseButtonEventArgs e)
        {
            textuserrr.Focus();
        }

        private void passmousee(object sender, MouseButtonEventArgs e)
        {
            textpasss.Focus();
        }



        private void key(object sender, MouseButtonEventArgs e)
        {
            keyword.Focus();
        }

        private void keychange(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(keyword.Text) && txtkey.Password.Length > 0)
                keyword.Visibility = Visibility.Collapsed;
            else
                keyword.Visibility = Visibility.Visible;
        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {

            textuserrr.TextAlignment.Equals(1);

        }

        private void passchangedd(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textpasss.Text) && txtpasss.Text.Length > 0)
                textpasss.Visibility = Visibility.Collapsed;
            else
                textpasss.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            if (string.IsNullOrEmpty(txtpasss.Text) && string.IsNullOrEmpty(txtuserrr.Text))
            {
                MessageBox.Show("برجاء ادخال اسم المستخدم وكلمة المرور ");
                return;
            }
            else if (string.IsNullOrEmpty(txtpasss.Text))
            {
                MessageBox.Show("برجاء ادخال كلمة المرور الجديدة");
                return;
            }
            else if (string.IsNullOrEmpty(txtuserrr.Text))
            {
                MessageBox.Show("برجاء ادخال اسم المستخدم");
                return;
            }
            else if (string.IsNullOrEmpty(txtkey.Password))
            {
                MessageBox.Show("برجاء ادخال رقم المفتاح الخاص بكلمة المرور ");
                return;
            }
            if (!(string.IsNullOrEmpty(txtpasss.Text) && string.IsNullOrEmpty(txtuserrr.Text)))
                if (txtkey.Password == "333334")
                {
                    admin d = new admin();
                    d.username = txtuserrr.Text;
                    d.password = txtpasss.Text;


                    db.admins.Add(d);

                    db.SaveChanges();
                    txtuserrr.Text = "";
                    txtpasss.Text = "";
                    MessageBox.Show("تم التسجيل بنجاح");
                    MainWindow m = new MainWindow();
                    m.Show();
                    this.Close();
                }
                else
                    MessageBox.Show("رقم المفتاح خاطي   برجاء ادخال رقم المفتاح الصحيح ");
        }
    }
}

