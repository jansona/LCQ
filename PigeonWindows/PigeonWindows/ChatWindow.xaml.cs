using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UDPClient;

namespace PigeonWindows
{
    /// <summary>
    /// ChatWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ChatWindow : Window
    {
        private UdpHandler handler;

        public String Message { set; get; }

        public ChatWindow()
        {
            InitializeComponent();

            handler = new UdpHandler(this);

            this.DataContext = new ChatWindowViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //handler.SendMessage("192.168.99.1", "19966", "test message");
            //textBox.Text = "send";
            handler.TestSend();
        }

        public void Response(String message)
        {
            textBox.Text = message;
            
        }
    }
}
