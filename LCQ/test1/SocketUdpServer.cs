using LCQ;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace test1
{
    class SocketUdpServer
    {
        private Thread listenThread;

        private Socket listenSocket;

        private int port;

        private string message;

        private EndPoint remoteEndPoint;
        #region Method

        #region 停止当前监听和断开线程
        /// <summary>
        /// 停止当前服务器的监听和断开线程
        /// </summary>
        public void Stop()
        {
            this.listenThread.Abort();
            this.listenSocket.Close();
        }
        #endregion

        #region 监听

        /// <summary>
        /// 开始监听
        /// </summary>
        public void Listen()
        {
            ThreadStart method = new ThreadStart(this.ListenMethod);
            this.listenThread = new Thread(method);
            this.listenThread.Start();
        }

        /// <summary>
        /// 监听的方法
        /// </summary>
        private void ListenMethod()
        {
            try
            {
                this.listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, this.port);
                this.listenSocket.Bind(ipep);//定义一个网络端点

                IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);//定义要发送的计算机的地址
                EndPoint remote = (EndPoint)(sender);//远程

                ///持续监听
                while (true)
                {
                    byte[] data = new byte[1024];

                    //准备接收
                    int recv = this.listenSocket.ReceiveFrom(data, ref remote);
                    string stringData = Encoding.UTF8.GetString(data, 0, recv);
                    //将接收到的信息转化为自定义的数据报类
                    Datagram recvicedataGram = Datagram.Convert(stringData);
                    this.message = recvicedataGram.Message;
                    string remotePoint = remote.ToString();
                    string remoteip = remotePoint.Substring(0, remotePoint.IndexOf(":"));
                    remote = new IPEndPoint(IPAddress.Parse(remoteip), this.port);
                    this.remoteEndPoint = remote;
                    this.Action(recvicedataGram.Type);

                }
            }
            catch (Exception ex)
            {
                this.message = ex.Message;
                this.ErrorAppear(this, new EventArgs());
            }
        }

        /// <summary>
        /// 收到数据报后的动作
        /// </summary>
        /// <param name="type">数据报的类型</param>
        private void Action(DatagramType type)
        {
            switch (type)
            {
                case DatagramType.OnLine:
                    Datagram sendDataGram = new Datagram
                    {
                        Type = DatagramType.GiveInfo,
                        FromAddress = "",
                        ToAddress = "",
                        Message = Dns.GetHostName()
                    };
                    //告诉对方自己的信息
                    this.listenSocket.SendTo(Encoding.UTF8.GetBytes(sendDataGram.ToString()), this.remoteEndPoint);
                    this.OnLineComplete(this, new EventArgs());
                    break;
                case DatagramType.GiveInfo:
                    ///执行添加上线用户事件
                    this.OnLineComplete(this, new EventArgs());
                    break;
                case DatagramType.DownLine:
                    ///执行用户下线事件
                    ///如果是自己下线
                    if (string.Compare(Dns.GetHostName(), message) == 0)
                    {
                        //System.Windows.Forms.Application.Exit();
                    }
                    else
                    {
                        this.DownLineComplete(this, new EventArgs());
                    }
                    break;
                case DatagramType.Chat:
                    //得到当前要交谈的用户
                    LanInfo lanInfo = LanList.CurrentLanList.Find(x => string.Compare(this.remoteEndPoint.ToString(), x.RemoteEndPoint.ToString()) == 0);
                    //如果有查询到该用户在自己这边登记过
                    if (lanInfo != null)
                    {

                        if (lanInfo.State == TalkState.Talking)
                        {
                            //正在交谈 直接打开这次窗口
                            this.OnChatComplete(this, new EventArgs());
                        }
                        else
                        {
                            //没有交谈 将窗口加入信息的队列
                            MessageInfo messageInfo = new MessageInfo()
                            {
                                Message = this.message,
                                ReceiveTime = DateTime.Now,
                                RemoteEndPoint = this.remoteEndPoint
                            };
                            QueueMessage.Add(lanInfo.Host, messageInfo);
                        }
                    }
                    break;
            }
        }

        #endregion

        #endregion

        #region Delegate Event

        /// <summary>
        /// 完成一个socket的代理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void OnCompleteHander(SocketUdpServer sender, EventArgs e);
        /// <summary>
        /// 完成收到一个主机信息 即上线事件
        /// </summary>
        public event OnCompleteHander OnLineComplete;
        /// <summary>
        /// 完成下线事件
        /// </summary>
        public event OnCompleteHander DownLineComplete;
        /// <summary>
        /// 完成一次谈话  就一条信息
        /// </summary>
        public event OnCompleteHander OnChatComplete;
        /// <summary>
        /// 有错误出现
        /// </summary>
        public event OnCompleteHander ErrorAppear;

        #endregion
    }
}
