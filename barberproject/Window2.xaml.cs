using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {

        public Window2()
        {

            InitializeComponent();
        }


        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            make_invoice w = new make_invoice();
            w.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            password_win p = new password_win();
            p.Show();

        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            password_win p = new password_win();
            p.Show();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            price_list_win p = new price_list_win();
            p.Show();
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {

            inventory_Win n = new inventory_Win();
            n.Show();
            this.Close();
        }



        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }


        private void Button_Click_44(object sender, RoutedEventArgs e)
        {
            date w = new date();
            w.Show();
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Invoice_search w = new Invoice_search();
            w.Show();
            this.Close();


        }
    }
}




