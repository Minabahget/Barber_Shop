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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace barberproject
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        barberEntities1 db = new barberEntities1();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void usermousedown(object sender, MouseButtonEventArgs e)
        {
            textuserr.Focus();
        }

        private void userchanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textuserr.Text) && txtuserr.Text.Length > 0)
                textuserr.Visibility = Visibility.Collapsed;
            else
                textuserr.Visibility = Visibility.Visible;
        }

        private void passmouse(object sender, MouseButtonEventArgs e)
        {
            textpass.Focus();
        }

        private void passchanged(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(textpass.Text) && txtpass.Password.Length > 0)
                textpass.Visibility = Visibility.Collapsed;
            else
                textpass.Visibility = Visibility.Visible;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Window1 w = new Window1();
            w.Show();
            this.Close();

        }



        private void clicksignin(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtuserr.Text))
            {
                MessageBox.Show("برجاء ادخال اسم المستخدم");
            }
            else if (string.IsNullOrEmpty(txtpass.Password))
            {

                MessageBox.Show("برجاء ادخال كلمة المرور ");
            }
            else
            {
                var user = db.admins.Where(x => x.username == txtuserr.Text).FirstOrDefault();
                var pass = db.admins.Where(x => x.password == txtpass.Password).FirstOrDefault();
                if (user != null && pass != null)
                {
                    Window2 w = new Window2();
                    w.Show();
                    this.Close();
                }

                else if (user == null && pass == null)
                {
                    MessageBox.Show("اسم مستخدم خاطى وكلمة مرور خاطئة");

                }
                else if (user == null)
                {
                    MessageBox.Show("اسم مستخدم خاطى ");

                }

                else
                    MessageBox.Show("كلمة مرور خاطئة");

            }

        }

        private void Window_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}

