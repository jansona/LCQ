using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PigeonWindows
{
    //用户类包括Ip,头像，姓名
    //当解析得到传来的数据时，可将IP和姓名初始化user，头像自动初始化
    public class User
    {
        public string UserIp{get;set;}
        public string UserName { get; set; }
        public BitmapImage Head { get; set; }
        public User(string Ip,string Name)
        {
            UserIp = Ip;
            UserName = Name;
            //Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon1.jpg"));
        }
        public User()
        {
            //Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon1.jpg"));
        }

    }
}
