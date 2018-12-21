using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Xml.Serialization;

namespace PigeonWindows
{
    //用户类包括Ip,头像，姓名
    //当解析得到传来的数据时，可将IP和姓名初始化user，头像自动初始化
    //聊天记录保存在Message里
    [Serializable]
    public class User
    {
        public string UserIp { get; set; }
        public string UserName { get; set; }
        public BitmapImage Head { get; set; }
        //键是对方的ip地址，值是text文本内容
        public Message Messages { get; set; }
        public User(string Ip, string Name)
        {
            UserIp = Ip;
            UserName = Name;
            //Messages = new List<Message>();
            //Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon1.jpg"));
        }
        public User()
        {
            //Messages = new List<Message>();
            //Head = new BitmapImage(new Uri("pack://application:,,,/Images/icon1.jpg"));
        }



        public static void XmlSerialize(XmlSerializer ser, string fileName, object obj)
        {
            FileStream fs = new FileStream(fileName, FileMode.Create);
            try { ser.Serialize(fs, obj); }
            finally { fs.Close(); }


        }
        public static object XmlDeserialize(XmlSerializer ser, Stream stream)
        {
            return ser.Deserialize(stream);
        }
        public void Export()
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Message));
            string xmlFileName = UserName + "message" + ".xml";
            XmlSerialize(xmlSerializer, xmlFileName, Messages);
            Console.WriteLine("已保存所有数据");
        }
        public void Import()
        {
            if (!File.Exists(UserName + "message" + ".xml")) { Console.WriteLine("导入失败，本地无数据"); return; }
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(Message));
            FileStream fs = new FileStream(UserName + "message" + ".xml", FileMode.Open, FileAccess.Read);
            Message temp;
            try
            {
                temp = (Message)xmlSerializer.Deserialize(fs);
            }
            finally
            {
                fs.Close();
            }


            Messages = temp;

            Console.WriteLine("导入成功！");
        }
    }
}
