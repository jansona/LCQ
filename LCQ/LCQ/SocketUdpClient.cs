using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LCQ
{
    /// <summary>
    /// udp的客户端  主要用户发送数据
    /// </summary>
    public class SocketUdpClient
    {
        #region Feild

        /// <summary>
        /// 广播的socket
        /// </summary>
        private Socket broadcastSocket;

        /// <summary>
        /// 服务器的端口
        /// </summary>
        private int port;

        /// <summary>
        /// 远端的端点
        /// </summary>
        private EndPoint remoteEndPoint = null;
        /// <summary>
        /// 当前客户端
        /// </summary>
        private Socket client = null;

        #endregion

        #region Constructor

        /// <summary>
        /// 构造函数
        /// </summary>
        public SocketUdpClient(EndPoint point)
        {
            this.client = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            this.remoteEndPoint = point;
        }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public SocketUdpClient()
        {
            this.port = 9050;
        }

        #endregion

        #region 进行广播
        /// <summary>
        /// 进行广播 上线或者下线
        /// </summary>
        /// <param name="msg">广播中发送的信息</param>
        public void Broadcast(DatagramType type)
        {
            this.broadcastSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint iep = new IPEndPoint(IPAddress.Broadcast, this.port);
            this.broadcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            Datagram dataGram = new Datagram
            {
                Type = type,
                FromAddress = "",
                ToAddress = "",
                Message = Dns.GetHostName()
            };

            //将要发送的信息改为字节流
            byte[] data = Encoding.ASCII.GetBytes(dataGram.ToString());
            this.broadcastSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, 1);
            this.broadcastSocket.SendTo(data, iep);
            //this.broadcastSocket.Close();
        }

        #endregion

        #region Method

        /// <summary>
        /// 发送数据
        /// </summary>
        /// <param name="message">当前的数据</param>
        public void Send(string message)
        {
            byte[] data = Encoding.UTF8.GetBytes("CHAT" + message);

            int i = client.SendTo(data, this.remoteEndPoint);
        }

        #endregion

    }
}
