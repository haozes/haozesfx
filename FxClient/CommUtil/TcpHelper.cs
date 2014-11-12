using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using Haozes.FxClient.Sip;

namespace Haozes.FxClient.CommUtil
{
    public static class TcpHelper
    {
        public static Socket CreateSocket(string ip, string port)
        {
            Socket socket = null;

            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), int.Parse(port));
            socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            socket.Connect(endPoint);
            if (!socket.Connected)
            {
                socket = null;
            }
            return socket;
        }

        public static string GetTcpClientResult(string host, int port, string sendData)
        {
            TcpClient client = new TcpClient();

            string str = string.Empty;
            try
            {
                client.Connect(host, port);
                NetworkStream stream = client.GetStream();
                byte[] bytes = Encoding.UTF8.GetBytes(sendData);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                byte[] buffer = new byte[0x400];
                int count = stream.Read(buffer, 0, 0x400);
                str = Encoding.UTF8.GetString(buffer, 0, count);
                stream.Close();
                client.Close();
            }
            catch (Exception exception)
            {
                client.Close();
                Console.WriteLine(exception.ToString());
            }
            return str;
        }

        public static string GetTcpClientResult(string host, int port, string sendData, bool isWait)
        {
            TcpClient client = new TcpClient();
            bool flag = true;
            string str = string.Empty;
            try
            {
                client.Connect(host, port);
                NetworkStream stream = client.GetStream();
                byte[] bytes = Encoding.UTF8.GetBytes(sendData);
                stream.Write(bytes, 0, bytes.Length);
                stream.Flush();
                byte[] buffer = new byte[0x400];
                while (flag)
                {
                    int count = stream.Read(buffer, 0, 0x400);
                    str += Encoding.UTF8.GetString(buffer, 0, count);
                    if (str.IndexOf("</config>") > 0)
                        flag = false;
                }
                stream.Close();
                client.Close();
            }
            catch (Exception exception)
            {
                client.Close();
                Console.WriteLine(exception.ToString());
            }
            return str;
        }

        public static string SendAndReceive(Socket socket, string sendData)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(sendData);
            byte[] buffer = new byte[0x800];
            string result = string.Empty;

            int count = socket.Send(bytes);
            int receiveCount = socket.Receive(buffer);
            result = Encoding.UTF8.GetString(buffer);

            return result;
        }

        public static string SendAndReceive(Socket socket, string sendData, string waitTag, int tryCount)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(sendData);
            byte[] buffer = new byte[0x800];
            string result = string.Empty;
            int rcvCount = 0;
            try
            {
                int count = socket.Send(bytes);
                while (true && rcvCount < tryCount)
                {
                    int receiveCount = socket.Receive(buffer);
                    result += Encoding.UTF8.GetString(buffer);
                    if (result.IndexOf(waitTag) >= 0)
                        break;
                    rcvCount++;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            return result;
        }

        public static void Send(Socket socket, string sendData)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(sendData);
            int count = socket.Send(bytes);
        }

        public static void Send(Socket socket, SipMessage packet)
        {
            Send(socket, packet.ToString());
        }

        public static void AsyncSend(Socket socket, SipMessage packet, AsyncCallback callBack)
        {
            AsyncSend(socket, packet.ToString(), callBack);
        }

        public static void AsyncSend(Socket socket, string sendData, AsyncCallback callBack)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(sendData);
            socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, callBack, socket);
        }
    }
}
