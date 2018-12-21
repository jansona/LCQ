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
using UDPClient;

namespace PigeonWindows
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {

        //public String Message { set; get; }
        public UdpHandler handler;

        public String MyName { set; get; }

        public MainWindow()
        {
            InitializeComponent();
            MyName = "ha";
            handler = new UdpHandler(this);
            this.DataContext = new MainWindowViewModel(this);
        }

        private void NavBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //handler.SendMessage("192.168.99.1", "19966", "test message");
            //textBox.Text = "send";
            //handler.TestSend();
        }
        public void UpdateClientList(string remoteIP, string name, bool isOnline)
        {
            if (isOnline)
            {
                //MainWindowViewModel.Friends.Add(new User(remoteIP, name));

                Action updateUI = new Action(() => { MainWindowViewModel.Friends.Add(new User(remoteIP, name)); });
                Dispatcher.BeginInvoke(updateUI);
            }
            else
            {
                var query = from user in MainWindowViewModel.Friends
                            where user.UserIp == remoteIP
                            select user;

                Action updateUI = new Action(() =>
                {
                    MainWindowViewModel.Friends.Remove(query.ToList()[0]);
                });
                Dispatcher.BeginInvoke(updateUI);
            }
        }
        public void AppendMessageRecord(string remoteIP, string message)
        {
            var query = from user in MainWindowViewModel.Friends
                        where user.UserIp == remoteIP
                        select user;
            User targetUser = query.First();
            Action updateUI = new Action(() =>
            {
                targetUser.Messages.Text += ("\n" + message);
            });
            Dispatcher.BeginInvoke(updateUI);
        }
        public void InitClientList(List<User> list)
        {
            Action updateUI = new Action(() =>
            {
                foreach (User user in list)
                {
                    MainWindowViewModel.Friends.Add(user);
                }
            });
            Dispatcher.BeginInvoke(updateUI);
        }
    }
}
