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
    }
}
