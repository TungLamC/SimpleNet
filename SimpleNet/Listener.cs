using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNet
{
    /// <summary>
    /// 端口监听者
    /// </summary>
    public class Listener
    {
        IPEndPoint address = null;
        Socket socket = null; // 用于监听端口的socket

        private event ClientAccepted clientAcceptedEventHandler;
        private event ClientReceived clientReceivedEventHandler;
        private event ClientClosed clientClosedEventHandler;

        /// <summary>
        /// 构造方法中指定要监听的IP地址和端口号 并做好Bind和Listen的工作
        /// </summary>
        /// <param name="ip">要监听的IP地址</param>
        /// <param name="port">要监听的端口号</param>
        public Listener(string ip, int port)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            address = new IPEndPoint(ipAddress, port);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.Bind(address);
            socket.Listen(0);
        }

        #region 添加事件接口
        public void AddAccepted(ClientAccepted accepted)
        {
            clientAcceptedEventHandler += accepted;
        }
        public void AddReceived(ClientReceived received)
        {
            clientReceivedEventHandler += received;
        }
        public void AddClosed(ClientClosed closed)
        {
            clientClosedEventHandler += closed;
        }
        #endregion

        /// <summary>
        /// 监听者开始工作 异步接收客户端的连接请求 接收成功后开始异步接收客户端的数据
        /// </summary>
        public void Start()
        {
            StartAcceptAsync();
        }

        /// <summary>
        /// 开始异步接收客户端的连接请求
        /// </summary>
        private void StartAcceptAsync()
        {
            socket.BeginAccept(AcceptCallBack, null);
        }
        private void AcceptCallBack(IAsyncResult ar)
        {
            Socket clientSocket = socket.EndAccept(ar); // 请求连接的客户端socket

            // 触发客户端连接成功事件
            clientAcceptedEventHandler(clientSocket);

            ClientReceiveArgs clientReceiveArgs = new ClientReceiveArgs(clientSocket, new byte[1024]);
            StartReceiveClientAsync(clientReceiveArgs);

            StartAcceptAsync();
        }


        private void StartReceiveClientAsync(ClientReceiveArgs args)
        {
            args.socket.BeginReceive(args.buffer, 0, args.size, SocketFlags.None, ReceiveClientCallBack, args);
        }
        private void ReceiveClientCallBack(IAsyncResult ar)
        {
            ClientReceiveArgs args = ar.AsyncState as ClientReceiveArgs;
            try
            {
                // 接收到的数据长度
                int count = args.socket.EndReceive(ar);

                if (count == 0)
                {
                    // 触发客户端断开事件
                    clientClosedEventHandler(args.socket);
                    args.socket.Close();

                    return;
                }

                // 将获取到的数据提取到一个新的字节数组中
                byte[] data = new byte[count];
                Buffer.BlockCopy(args.buffer, 0, data, 0, count);

                // 触发接收数据成功事件
                clientReceivedEventHandler(args.socket, data);

                // 回调 继续接收客户端发送过来的数据
                StartReceiveClientAsync(args);
            }
            // 客户端异常关闭处理
            catch (System.Net.Sockets.SocketException e)
            {
                Console.WriteLine(e);
                // 触发客户端断开事件
                clientClosedEventHandler(args.socket);
                args.socket.Close();
            }

        }
    }

    class ClientReceiveArgs
    {
        internal Socket socket { get; set; } = null;
        internal byte[] buffer { get; set; } = null;
        internal int offset { get; set; } = -1;
        internal int size { get; set; } = 0;

        public ClientReceiveArgs(Socket socket, byte[] buffer, int offset)
        {
            this.socket = socket;
            this.buffer = buffer;
            this.offset = offset;
            this.size = buffer.Length;
        }

        public ClientReceiveArgs(Socket socket, byte[] buffer)
        {
            this.socket = socket;
            this.buffer = buffer;
            this.size = buffer.Length;
        }

    }
}
