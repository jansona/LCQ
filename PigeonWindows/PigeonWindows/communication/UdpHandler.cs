﻿using System;
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

        public UdpClient sendUdpClient;
        public UdpClient receiveUpdClient;
        
        public String SendPort { set; get; }
        public String ListenPort { set; get; }

        public UdpHandler(MainWindow window):this()
        {
            mainWindow = window;
            Broadcast(DatagramType.OnLine);
        }

        public UdpHandler()
        {
            SendPort = "6699";
            IPAddress localIp = IPAddress.Parse(GetLocalIP());
            IPEndPoint sendEndPoint = new IPEndPoint(localIp, int.Parse(SendPort));
            sendUdpClient = new UdpClient(sendEndPoint);

            //监听线程启动
            ListenPort = "9966";
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

        public delegate void UpdateUIDelegate(byte[] receiveBytes, IPEndPoint remoteIpEndPoint);
        private void ReceiveMessage()
        {
            IPEndPoint remoteIpEndPoint = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                try
                {
                    byte[] receiveBytes = receiveUpdClient.Receive(ref remoteIpEndPoint);
                    UpdateUIDelegate updateUIDelegate = new UpdateUIDelegate(update);

                    //通过调用委托
                    this.mainWindow.Dispatcher.BeginInvoke(updateUIDelegate, receiveBytes, remoteIpEndPoint);
                    
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                    break;
                }
            }
        }

        // 这里要调用一个委托，并利用上面的invoke实现线程的异步执行，否则会报错
        // 2018年12月28日02:33:28  调这个bug调了一个半小时  fxxxxxxxxxxxxxk
        private void update( byte[] receiveBytes, IPEndPoint remoteIpEndPoint)
        {

            string message1 = Encoding.Unicode.GetString(receiveBytes);

            string remoteIPAddress = remoteIpEndPoint.Address.ToString();
            if (remoteIPAddress != GetLocalIP())
                Datagram.Convert(message1, remoteIPAddress, mainWindow, sendUdpClient);
        }


        public void Broadcast(DatagramType type)
        {
            byte[] sendbytes = Encoding.Unicode.GetBytes(
                new Datagram(type.ToString(), mainWindow.MyName,mainWindow.MyIcon).ToString());
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Broadcast,
                int.Parse(ListenPort));
            sendUdpClient.Send(sendbytes, sendbytes.Length, remoteIPEndPoint);
        }

        public void Broadcast(DatagramType type,string message)
        {
            byte[] sendbytes = Encoding.Unicode.GetBytes(
                new Datagram(type.ToString(), message).ToString());
            IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Broadcast,
                int.Parse(ListenPort));
            sendUdpClient.Send(sendbytes, sendbytes.Length, remoteIPEndPoint);
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
    }
}
