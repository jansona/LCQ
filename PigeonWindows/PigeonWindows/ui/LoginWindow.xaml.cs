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

namespace PigeonWindows
{
    /// <summary>
    /// LoginWindow.xaml 的交互逻辑
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        public string myname = "";

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            myname = textBox.Text;
            if (myname == "")
            {
                textBox.Text = "用户名不能为空";
                return;
            }
            MainWindow mainWindow = new MainWindow(myname);
            mainWindow.Show();
            this.Close();
        }

        private void Key_Press(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                Button_Click(this, new RoutedEventArgs());
            }
        }

        private void NavBar_MouseLeftButtonDown0(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Button_Minimize0(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {            
            this.Close();
        }

        private void Button2_MouseEnter(object sender, MouseEventArgs e)
        {
            button2.Height = 18;
        }

        private void Button2_MouseLeave(object sender, MouseEventArgs e)
        {
            button2.Height = 31;
        }

        private void Button1_MouseEnter(object sender, MouseEventArgs e)
        {
            button1.Height = 18;
        }

        private void Button1_MouseLeave(object sender, MouseEventArgs e)
        {
            button1.Height = 31;
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            button.FontSize = button.FontSize + 5;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            button.FontSize = button.FontSize - 5;
        }
    }
}
