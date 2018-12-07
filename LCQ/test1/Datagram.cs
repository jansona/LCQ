using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LCQ
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
        /// 发送者的网络地址
        /// </summary>
        public string FromAddress
        {
            get;
            set;
        }

        /// <summary>
        /// 接收者网络地址
        /// </summary>
        public string ToAddress
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
            sb.AppendFormat("Type={0},", this.Type.ToString());
            sb.AppendFormat("FromAddress={0},", this.FromAddress.ToString());
            sb.AppendFormat("ToAddress={0},", this.ToAddress.ToString());
            sb.AppendFormat("Message={0}", this.Message.ToString());
            return sb.ToString();
        }

        /// <summary>
        /// 将有效字符串转化成数据报
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static Datagram Convert(string str)
        {
            Datagram data = new Datagram();
            //前面不是CHAT主要是建立连接 取消连接等信号传送
            if (!str.StartsWith("CHAT"))
            {
                IDictionary<string, string> idict = new Dictionary<string, string>();

                string[] strlist = str.Split(',');
                for (int i = 0; i < strlist.Length; i++)
                {
                    //数据报字符串的各个键值对放进字典类
                    string[] info = strlist[i].Split('=');
                    idict.Add(info[0], info[1]);
                }

                data.Type = (DatagramType)Enum.Parse(typeof(DatagramType), idict["Type"]);
                data.FromAddress = idict["FromAddress"];
                data.ToAddress = idict["ToAddress"];

                data.Message = idict["Message"];
            }
            else
            {
                data.Type = (DatagramType)Enum.Parse(typeof(DatagramType), "Chat");
                data.Message = str.Substring(4);
            }

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
