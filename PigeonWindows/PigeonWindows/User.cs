using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace PigeonWindows
{
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
          
        }
        
    }
}
