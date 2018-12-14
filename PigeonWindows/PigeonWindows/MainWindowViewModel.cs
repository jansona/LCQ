﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace PigeonWindows
{
    class MainWindowViewModel : BindableBase
    {

        //#region class
        //public class Friend
        //{
        //    public string Nickname { get; set; }
        //    public BitmapImage Head { get; set; }
        //    public Friend(string name) {
        //        Nickname = name;
        //       // Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon1.jpg"));
        //    }
        //}
        //#endregion

        #region attributes
        public ObservableCollection<User> Friends { get; set; }
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
        #endregion

        #region delegates
        //Item点击事件
        public DelegateCommand<object> SelectItemChangedCommand { get; set; }
        //窗口关闭
        public DelegateCommand CloseCommand { get; set; }
        #endregion

        //更新friends
        public  List<User> FriendsChange(List<User> users) {
            Friends.Clear();
            foreach (User user in users) {
                Friends.Add(user);
            }
            return users;
        }
        //当friends更新后，更新friends的ui

        #region constructor
        public MainWindowViewModel()
        {
            Friends = new ObservableCollection<User>();
            //Friends.Add(new Friend() { Nickname = "Fear of god!", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon1.jpg")) });
            //Friends.Add(new Friend() { Nickname = "Fear of goddness!", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon2.jpg")) });
            //Friends.Add(new Friend() { Nickname = "欧阳铁柱", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon3.jpg")) });
            //Friends.Add(new Friend() { Nickname = "皇甫二妞", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon4.jpg")) });
            //Friends.Add(new Friend() { Nickname = "王二狗", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon5.jpg")) });
            //Friends.Add(new Friend() { Nickname = "幺妹", Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon6.jpg")) });

            CloseCommand = new DelegateCommand(() => {
                Application.Current.Shutdown();
            });
            SelectItemChangedCommand = new DelegateCommand<object>((p) => {
                ListView lv = p as ListView;
                User friend = lv.SelectedItem as User;
                Head = friend.Head;
                Nickname = friend.UserName;
            });
        }
        #endregion
    }
}
