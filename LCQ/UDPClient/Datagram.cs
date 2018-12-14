using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UDPClient
{
    /*****************************************************************
     * 定义广播的数据格式
     * Type=OnLine,FromAdress=xxx,ToAdress=zzz,Message=mmm
     * 类型为上线广播  从xxx主机到zzz主机  信息是mmm       
     * CHAT这个就是我的信息我的信息 可能有各种=,的字符串
     * 这种就直接将CHAT去掉后 后面的都为mmm
    *****************************************************************/

    /// <summary>
    /// 定义数据报里面的几个字段
    /// </summary>
    public class Datagram
    {
        #region Property

        /// <summary>
        /// 数据报的类型 ,
        /// </summary>
        public DatagramType Type
        {
            get;
            set;
        }


        /// <summary>
        /// 数据报的信息
        /// </summary>
        public string Message
        {
            get;
            set;
        }

        /// <summary>
        /// 信息 Message的长度
        /// </summary>
        public int Length
        {
            get
            {
                return Message.Length;
            }
        }

        #endregion

        #region Method

        /// <summary>
        /// 重写下ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( this.Type.ToString());
            sb.Append( this.Message.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// 将有效字符串转化成数据报
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Datagram CreatDatagram(DatagramType type)
        {
            Datagram data = new Datagram();
            data.Type = type;
            return data;
        }

        public static Datagram CreatDatagram(string str)
        {
            Datagram data = new Datagram();
            data.Type = (DatagramType)Enum.Parse(typeof(DatagramType), "Chat");
            data.Message = str;

            return data;
        }

        public static Datagram CreatDatagram(string str, DatagramType type)
        {
            Datagram data = new Datagram();
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

            return data;
        }

        #endregion
    }

    #region Enum

    /// <summary>
    /// 数据报的类型
    /// </summary>
    public enum DatagramType
    {
        /// <summary>
        /// 上线  一应一答
        /// </summary>
        OnLine = 1,
        /// <summary>
        /// 下线 一应
        /// </summary>
        DownLine,
        /// <summary>
        /// 确认收到 一应
        /// </summary>
        /// <summary>
        /// 正常聊天 一应一答
        /// </summary>
        Chat,
        /// <summary>
        /// 给予个人的信息
        /// </summary>
        GiveInfo

    }

    #endregion
}
