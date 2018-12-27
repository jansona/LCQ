﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

        public Datagram(string type, string message)
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
                sb.Append(user.UserIp);
                sb.Append(",");
                sb.Append(user.UserName);
                sb.Append(",");
                sb.Append(user.IconName);
                sb.Append(",");
            }
            Type = (DatagramType)Enum.Parse(typeof(DatagramType), "UserList");
            Message = sb.ToString();
        }

        public Datagram(string type, string name, string icon)
        {

            Message = name + "," + icon;
            Type = Type = (DatagramType)Enum.Parse(typeof(DatagramType), type);
        }

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

        public static void Convert(string dataStr, string ip, MainWindow window, UdpClient sendUdpClient)
        {
            Datagram data = GetDatagramFromStr(dataStr);
            User user = GetUser(data);
            switch ((int)data.Type)
            {
                case 1:
                    window.UpdateClientList(ip, user.UserName,user.IconName, true);

                    List<User> users = MainWindowViewModel.Friends.ToList();
                    users.Add(new User(UDPClient.UdpHandler.GetLocalIP(), window.MyName));
                    byte[] sendbytes = Encoding.Unicode.GetBytes(
                         new Datagram(users).ToString());
                    IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Parse(ip),
                        9966);
                    sendUdpClient.Send(sendbytes, sendbytes.Length, remoteIPEndPoint);
                    break;
                case 2:
                    window.UpdateClientList(ip, user.UserName,user.IconName, false);
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
            string[] strList = data.Message.Split(',');
            for (int i = 0; i < strList.Length / 3; i++)
            {
                users.Add(new User(strList[3 * i], strList[3 * i + 1], strList[3 * i + 2]));
            }
            return users;
        }

        public static User GetUser(Datagram data)
        {
            User user = new User();
            string[] strList = data.Message.Split(',');
            user.UserName = strList[0];
            user.IconName = strList[1];
            return user;
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
