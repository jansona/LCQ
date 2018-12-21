using System;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using PigeonWindows;

namespace UDPClient
{
    public partial class UdpHandler
    {
        private MainWindow mainWindow;

        private UdpClient sendUdpClient;
        private UdpClient receiveUpdClient;
        
        public String SendPort { set; get; }
        public String ListenPort { set; get; }

        public UdpHandler(MainWindow window):this()
        {
            mainWindow = window;

            Broadcast(DatagramType.OnLine);
        }

        public UdpHandler()
        {
            SendPort = "9966";
            IPAddress localIp = IPAddress.Parse(GetLocalIP());
            IPEndPoint sendEndPoint = new IPEndPoint(localIp, int.Parse(SendPort));
            sendUdpClient = new UdpClient(sendEndPoint);

            ListenPort = "19966";
            IPEndPoint listenEndPoint = new IPEndPoint(localIp, int.Parse(ListenPort));
            receiveUpdClient = new UdpClient(listenEndPoint);
            Thread receiveThread = new Thread(ReceiveMessage);
            receiveThread.Start();            
        }

        public void TestSend()
        {
            SendMessage(GetLocalIP(), "19966", "test message");
        }

        public void SendMessage(String ipString, String port, String message)
        {
            IPAddress remoteIP = IPAddress.Parse(ipString);
            SendMessage(remoteIP, port, message);
        }

        public void SendMessage(IPAddress remoteIP, String port, String message)
        {
            byte[] sendbytes = Encoding.Unicode.GetBytes(message);
            IPEndPoint remoteIpEndPoint = new IPEndPoint(remoteIP, int.Parse(port));
            sendUdpClient.Send(sendbytes, sendbytes.Length, remoteIpEndPoint);

            // 多线程修改UI示例
            //Action<MainWindow> updateUI = new Action<MainWindow>((w) => { ((MainWindow)w).textBox.Text = "has sent."; });
            //MainWindow.Dispatcher.BeginInvoke(updateUI, MainWindow);
        }

        private void ReceiveMessage()
        {
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 9966);
            while (true)
            {
                try
                {
                    // 关闭receiveUdpClient时此时会产生异常
                    byte[] receiveBytes = receiveUpdClient.Receive(ref remoteIpEndPoint);

                    string message = Encoding.Unicode.GetString(receiveBytes);

                    Datagram.Convert(message, remoteIpEndPoint.Address.ToString(), mainWindow);

                }
                catch
                {
                    break;
                }
            }
        }

        public void Broadcast(DatagramType type)
        {
            //IPAddress[] userList;
            
            //string announcement = "ONLINE";
            byte[] sendbytes = Encoding.Unicode.GetBytes(
                new Datagram(type.ToString(), mainWindow.MyName).ToString());
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Broadcast,
                int.Parse(ListenPort));
            sendUdpClient.Send(sendbytes, sendbytes.Length, remoteIPEndPoint);

            //remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 19966);
            //byte[] receiveBytes = receiveUpdClient.Receive(ref remoteIPEndPoint);
            //string message = Encoding.Unicode.GetString(receiveBytes);

            // userList = new IPAddress[5];    //xjb写的，留着等黄卜江负责的解析接口

            //return userList;
        }



        public static string GetLocalIP()
        {
            try
            {

                string HostName = Dns.GetHostName(); //得到主机名
                IPHostEntry IpEntry = Dns.GetHostEntry(HostName);
                for (int i = 0; i < IpEntry.AddressList.Length; i++)
                {
                    //从IP地址列表中筛选出IPv4类型的IP地址
                    //AddressFamily.InterNetwork表示此IP为IPv4,
                    //AddressFamily.InterNetworkV6表示此地址为IPv6类型
                    if (IpEntry.AddressList[i].AddressFamily == AddressFamily.InterNetwork)
                    {
                        string ip = "";
                        ip = IpEntry.AddressList[i].ToString();
                        return IpEntry.AddressList[i].ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            //string result = RunApp("route", "print", true);
            //Match m = Regex.Match(result, @"0.0.0.0\s+0.0.0.0\s+(\d+.\d+.\d+.\d+)\s+(\d+.\d+.\d+.\d+)");
            //if (m.Success)
            //{
            //    return m.Groups[2].Value;
            //}
            //else
            //{
            //    try
            //    {
            //        System.Net.Sockets.TcpClient c = new System.Net.Sockets.TcpClient();
            //        c.Connect("www.baidu.com", 80);
            //        string ip = ((System.Net.IPEndPoint)c.Client.LocalEndPoint).Address.ToString();
            //        c.Close();
            //        return ip;
            //    }
            //    catch (Exception)
            //    {
            //        return null;
            //    }
            //}
        }

        public static string RunApp(string filename, string arguments, bool recordLog)
        {
            try
            {
                if (recordLog)
                {
                    Trace.WriteLine(filename + " " + arguments);
                }
                Process proc = new Process();
                proc.StartInfo.FileName = filename;
                proc.StartInfo.CreateNoWindow = true;
                proc.StartInfo.Arguments = arguments;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.UseShellExecute = false;
                proc.Start();

                using (System.IO.StreamReader sr = new System.IO.StreamReader(proc.StandardOutput.BaseStream, Encoding.Default))
                {
                    //string txt = sr.ReadToEnd();
                    //sr.Close();
                    //if (recordLog)
                    //{
                    //    Trace.WriteLine(txt);
                    //}
                    //if (!proc.HasExited)
                    //{
                    //    proc.Kill();
                    //}
                    //上面标记的是原文，下面是我自己调试错误后自行修改的
                    Thread.Sleep(100);           //貌似调用系统的nslookup还未返回数据或者数据未编码完成，程序就已经跳过直接执行
                                                 //txt = sr.ReadToEnd()了，导致返回的数据为空，故睡眠令硬件反应
                    if (!proc.HasExited)         //在无参数调用nslookup后，可以继续输入命令继续操作，如果进程未停止就直接执行
                    {                            //txt = sr.ReadToEnd()程序就在等待输入，而且又无法输入，直接掐住无法继续运行
                        proc.Kill();
                    }
                    string txt = sr.ReadToEnd();
                    sr.Close();
                    if (recordLog)
                        Trace.WriteLine(txt);
                    return txt;
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return ex.Message;
            }
        }


        // 接受消息
        //private void btnReceive_Click(object sender, EventArgs e)
        //{
        //    // 创建接收套接字
        //    IPAddress localIp = IPAddress.Parse(tbxlocalip.Text);
        //    IPEndPoint localIpEndPoint = new IPEndPoint(localIp, int.Parse(tbxlocalPort.Text));
        //    receiveUpdClient = new UdpClient(localIpEndPoint);


        //    Thread receiveThread = new Thread(ReceiveMessage);
        //    receiveThread.Start();
        //}

        // 接收消息方法
        //private void ReceiveMessage()
        //{
        //    IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
        //    while (true)
        //    {
        //        try
        //        {
        //            // 关闭receiveUdpClient时此时会产生异常
        //            byte[] receiveBytes = receiveUpdClient.Receive(ref remoteIpEndPoint);

        //            string message = Encoding.Unicode.GetString(receiveBytes);

        //            // 显示消息内容
        //            ShowMessageforView(lstbxMessageView, string.Format("{0}[{1}]", remoteIpEndPoint, message));
        //        }
        //        catch
        //        {
        //            break;
        //        }
        //    }
        //}

        // 利用委托回调机制实现界面上消息内容显示
        //delegate void ShowMessageforViewCallBack(ListBox listbox, string text);
        //private void ShowMessageforView(ListBox listbox, string text)
        //{
        //    if (listbox.InvokeRequired)
        //    {
        //        ShowMessageforViewCallBack showMessageforViewCallback = ShowMessageforView;
        //        listbox.Invoke(showMessageforViewCallback, new object[] { listbox, text });
        //    }
        //    else
        //    {
        //        lstbxMessageView.Items.Add(text);
        //        lstbxMessageView.SelectedIndex = lstbxMessageView.Items.Count - 1;
        //        lstbxMessageView.ClearSelected();
        //    }
        //}
        //private void btnSend_Click(object sender, EventArgs e)
        //{
        //    if (tbxMessageSend.Text == string.Empty)
        //    {
        //        MessageBox.Show("发送内容不能为空","提示");
        //        return;
        //    }

        //    // 选择发送模式
        //    if (chkbxAnonymous.Checked == true)
        //    {
        //        // 匿名模式(套接字绑定的端口由系统随机分配)
        //        sendUdpClient = new UdpClient(0);
        //    }
        //    else
        //    {
        //        // 实名模式(套接字绑定到本地指定的端口)
        //        IPAddress localIp = IPAddress.Parse(GetLocalIP());
        //        IPEndPoint localIpEndPoint = new IPEndPoint(localIp, int.Parse(tbxlocalPort.Text));
        //        sendUdpClient = new UdpClient(localIpEndPoint);
        //    }

        //    Thread sendThread = new Thread(SendMessage);
        //    sendThread.Start(tbxMessageSend.Text);
        //}

        // 发送消息方法
        //private void SendMessage(object obj)
        //{
        //    string message = (string)obj;
        //    byte[] sendbytes = Encoding.Unicode.GetBytes(message);
        //    IPAddress remoteIp = IPAddress.Parse(tbxSendtoIp.Text);
        //    //IPEndPoint remoteIpEndPoint = new IPEndPoint(remoteIp, int.Parse(tbxSendtoport.Text));
        //    IPEndPoint remoteIpEndPoint = new IPEndPoint(remoteIp, int.Parse(tbxSendtoport.Text));
        //    sendUdpClient.Send(sendbytes, sendbytes.Length, remoteIpEndPoint);
        //   // IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Parse("192.168.0.255"), int.Parse(tbxSendtoport.Text));
        //    //sendUdpClient.Connect(IPAddress.Parse("192.168.0.255"), int.Parse(tbxlocalPort.Text));

        //    sendUdpClient.Close();

        //    // 清空发送消息框
        //    ResetMessageText(tbxMessageSend);
        //}

        // 采用了回调机制
        // 使用委托实现跨线程界面的操作方式
        //delegate void ResetMessageCallback(TextBox textbox);
        //private void ResetMessageText(TextBox textbox)
        //{
        //    // Control.InvokeRequired属性代表
        //    // 如果空间的处理与调用线程在不同线程上创建的，则为true,否则为false
        //    if (textbox.InvokeRequired)
        //    {
        //        ResetMessageCallback resetMessagecallback = ResetMessageText;
        //        textbox.Invoke(resetMessagecallback, new object[] { textbox });
        //    }
        //    else
        //    {
        //        textbox.Clear();
        //        textbox.Focus();
        //    }
        //}
    }
}
