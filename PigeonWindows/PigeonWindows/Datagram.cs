using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PigeonWindows
{
    public class Datagram
    {
        public DatagramType Type
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public int Length
        {
            get
            {
                return Message.Length;
            }
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((int)this.Type);
            sb.Append(this.Message);
            return sb.ToString();
        }

        public Datagram(string type,string message)
        {
            Type = Type = (DatagramType)Enum.Parse(typeof(DatagramType), type);
            Message = message;
        }

        public Datagram()
        {

        }

        public Datagram(List<User> users)
        {
            StringBuilder sb = new StringBuilder();
            foreach (User user in users)
            {
                sb.Append(user.UserName);
                sb.Append(",");
                sb.Append(user.UserIp);
                sb.Append(",");
            }
            Type = (DatagramType)Enum.Parse(typeof(DatagramType), "UserList");
            Message = sb.ToString();
        }

        //public static Datagram CreatDatagram(string str, DatagramType type)
        //{
        //    Datagram data = new Datagram();
            //前面不是CHAT主要是建立连接 取消连接等信号传送
            //if (!isChat)
            //{
            //    IDictionary<string, string> idict = new Dictionary<string, string>();

            //    string[] strlist = str.Split(',');
            //    for (int i = 0; i < strlist.Length; i++)
            //    {
            //        //数据报字符串的各个键值对放进字典类
            //        string[] info = strlist[i].Split('=');
            //        idict.Add(info[0], info[1]);
            //    }

            //    data.Type = (DatagramType)Enum.Parse(typeof(DatagramType), idict["Type"]);

            //    data.Message = idict["Message"];
            //}
            //else
            //{
            //    data.Type = (DatagramType)Enum.Parse(typeof(DatagramType), "Chat");
            //    data.Message = str;
            //}

        //    return data;
        //}

        public static Datagram GetDatagramFromStr(string str)
        {
            string type = str.Substring(0, 1);
            string message = str.Substring(1);
            Datagram data = new Datagram
            {
                Type = (DatagramType)Enum.Parse(typeof(DatagramType), type),
                Message = message
            };
            return data;
        }

        public static void Convert(string dataStr,string ip,MainWindow window)
        {
            Datagram data = GetDatagramFromStr(dataStr);
            switch ((int)data.Type)
            {
                case 1:
                    window.UpdateClientList(ip,data.Message, true);
                    break;
                case 2:
                    window.UpdateClientList(ip,data.Message, true);
                    break;
                case 3:
                    window.AppendMessageRecord(ip, data.Message);
                    break;
                case 4:
                    window.InitClientList(GetUsers(data));
                    break;
            }
        }

        public static List<User> GetUsers(Datagram data)
        {
            List<User> users = new List<User>();
            string []strList = data.Message.Split(',');
            for(int i = 0; i < strList.Length / 2; i++)
            {
                users.Add(new User(strList[2 * i], strList[2 * i + 1]));
            }
            return users;
        }
    }

    public enum DatagramType
    {
        OnLine = 1,
        DownLine,
        Chat,
        UserList
    }
}
