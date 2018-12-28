using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.ComponentModel;
using UDPClient;

namespace PigeonWindows
{
    class MainWindowViewModel : BindableBase
    {

        public MainWindow window { get; set; }

        #region attributes
        public static ObservableCollection<User> Friends { get; set; }
        //名片中的头像和昵称数据源
        private BitmapImage head;
        public BitmapImage Head
        {
            get { return head; }
            set { SetProperty(ref head, value); }
        }
        private string nickname;
        public string Nickname
        {
            get { return nickname; }
            set { SetProperty(ref nickname, value); }
        }
        private string message;
        public string Message
        {
            get { return message; }
            set
            {
                SetProperty(ref message, value);
            }
        }
        private User friend;
        public User Friend
        {
            get { return friend; }
            set { SetProperty(ref friend, value);  }
        }
        #endregion

        #region delegates
        //Item点击事件
        public DelegateCommand<object> SelectItemChangedCommand { get; set; }
        //窗口关闭
        public DelegateCommand CloseCommand { get; set; }
        //添加好友
        public DelegateCommand AddCommand { get; set; }
        //发送消息
        public DelegateCommand SendMessageCommand { get; set; }
        #endregion

        #region public
        //更新friends
        public  List<User> FriendsChange(List<User> users) {
            Friends.Clear();
            foreach (User user in users) {
                Friends.Add(user);
            }
            return users;
        }
        //当friends更新后，更新friends的ui


       
        //当关闭聊天界面时，导出聊天记录
        
        #endregion

        #region constructor
        public MainWindowViewModel(MainWindow window)
        {
            this.window = window;

            Friends = new ObservableCollection<User>();
            //Friends.Add(new Friend() { Nickname = "Fear of god!", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon1.jpg")) });
            //Friends.Add(new Friend() { Nickname = "Fear of goddness!", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon2.jpg")) });
            //Friends.Add(new Friend() { Nickname = "欧阳铁柱", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon3.jpg")) });
            //Friends.Add(new Friend() { Nickname = "皇甫二妞", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon4.jpg")) });
            //Friends.Add(new Friend() { Nickname = "王二狗", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon5.jpg")) });
            //Friends.Add(new User() { UserName = "幺妹", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon6.jpg")), Messages = new Message("幺妹！" + '\n') });
            Friends.Add(new User() { UserName = "用于测试的我",
                UserIp = "192.168.43.111",
                Messages = new Message("<h1>测试消息</h1>" + '\n'),
                Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon6.jpg")),
                IconName = "icon6"
            });
            CloseCommand = new DelegateCommand(() => {
                Application.Current.Shutdown();
                window.handler.receiveUpdClient.Close();
                window.handler.sendUdpClient.Close();
            });

            SelectItemChangedCommand = new DelegateCommand<object>((p) => {
                ListView lv = p as ListView;
                Friend = lv.SelectedItem as User;
                Head = Friend.Head;
                Nickname = Friend.UserName;
                Friend.Import();
                Message = Friend.Messages.Text;
            });
            
            AddCommand = new DelegateCommand(() => {
                Friends.Add(new User() { UserName = "王二狗", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon5.jpg")) });
            });

            SendMessageCommand = new DelegateCommand(() => {
               Message =Friend.Messages.Text;
            });
        }
        #endregion
    }
}
