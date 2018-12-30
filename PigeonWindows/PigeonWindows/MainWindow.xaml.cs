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
        public UdpHandler handler;

        public String MyName { set; get; }
        public String MyIcon { get; set; }
        private bool isInGroupChat = false;

        
        public MainWindow(String myname)
        {
            MyName = myname;
            Random ran = new Random();
            MyIcon = "icon" + ran.Next(1, 7);
            InitializeComponent();
            handler = new UdpHandler(this);
            this.DataContext = new MainWindowViewModel(this);
        }

        private void NavBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
        private void Button_Minimize(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            User friend = FriendList.SelectedItem as User;

            TextRange textRange1 = new TextRange(
                        MessageBox.Document.ContentStart,
                        MessageBox.Document.ContentEnd
                        );
            if (textRange1.Text == "")
                return;
            friend.Messages.Text += "我: "+textRange1.Text+"\n";
            friend.Export();
            var data = new Datagram(DatagramType.Chat.ToString(), textRange1.Text);
            if (friend.UserName != "多人聊天")
                handler.SendMessage(friend.UserIp, "9966", data.ToString());
            else
                handler.Broadcast(DatagramType.GroupChat, textRange1.Text);

            // 重新给绑定属性赋值
            ((MainWindowViewModel)DataContext).Message = friend.Messages.Text;
            MessageBox.Document.Blocks.Clear();
        }
        public void UpdateClientList(string remoteIP, string name, string icon, bool isOnline)
        {
            if (isOnline)
            {
                Action updateUI = new Action(() => { MainWindowViewModel.Friends.Add(new User(remoteIP, name, icon)); });
                Dispatcher.BeginInvoke(updateUI);
            }
            else
            {
                var query = from user in MainWindowViewModel.Friends
                            where user.UserIp == remoteIP
                            select user;

                Action updateUI = new Action(() =>
                {
                    try
                    {
                        MainWindowViewModel.Friends.Remove(query.ToList()[0]);
                    }
                    catch(Exception e)
                    {
                        Console.WriteLine(e);
                    }
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
            targetUser.Messages.Text += (targetUser.UserName + " : " + message + "\n");
            targetUser.Export();
            User currentUser = FriendList.SelectedItem as User;
            if (currentUser == targetUser)
            {
                MainWindowViewModel viewModel = DataContext as MainWindowViewModel;
                viewModel.Message = targetUser.Messages.Text;
            }
        }
        public void AppendMessageRecord(string groupchatip, string userip, string message)
        {
            var query = from user in MainWindowViewModel.Friends
                        where user.UserIp == groupchatip
                        select user;
            var query2 = from user in MainWindowViewModel.Friends
                        where user.UserIp == userip
                         select user;
            if (query.ToList().Count==0)
                return;
            User groupChat = query.First();
            User remoteUser = query2.First();
            groupChat.Messages.Text += (remoteUser.UserName + " : " + message + "\n");
            groupChat.Export();
            User currentUser = FriendList.SelectedItem as User;
            if (currentUser == groupChat)
            {
                MainWindowViewModel viewModel = DataContext as MainWindowViewModel;
                viewModel.Message = groupChat.Messages.Text;
            }
        }
        public void InitClientList(List<User> list)
        {
            Action updateUI = new Action(() =>
            {
                foreach (User user in list)
                {
                    var query = from user2 in MainWindowViewModel.Friends
                                where user2.UserIp == user.UserIp
                                select user2;
                    if (query.ToList().Count == 0&&user.UserName!= "多人聊天")
                        MainWindowViewModel.Friends.Add(user);
                }
            });
            Dispatcher.BeginInvoke(updateUI);
        }

        private void MessageBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(this, new RoutedEventArgs());
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!isInGroupChat)
            {
                Action updateUI = new Action(() =>
                {
                    MainWindowViewModel.Friends.Add(new User("0.0.0.0", "多人聊天", "groupchat"));
                });
                Dispatcher.BeginInvoke(updateUI);
                isInGroupChat = true;
            }
            else
            {
                var query = from user in MainWindowViewModel.Friends
                            where user.UserName == "多人聊天"
                            select user;
                Action updateUI = new Action(() =>
                {
                    MainWindowViewModel.Friends.Remove(query.ToList()[0]);
                });
                Dispatcher.BeginInvoke(updateUI);
                isInGroupChat = false;
            }

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            handler.Broadcast(DatagramType.DownLine);
            handler.receiveUpdClient.Close();
            handler.sendUdpClient.Close();
            Close();
        }

        private void ShowBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ShowBox.ScrollToEnd();
        }
    }
}
