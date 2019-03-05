using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SimpleNet
{
    /// <summary>
    ///  接收客户端连接请求成功后，触发的事件模板委托
    /// </summary>
    /// <param name="clientSocket">客户端连接Socket</param>
    public delegate void ClientAccepted(Socket clientSocket);

    /// <summary>
    /// 客户端数据接收成功后，触发的事件模板委托
    /// </summary>
    /// <param name="socket">接收数据所用到的socket</param>
    /// <param name="data">接收到的数据</param>
    public delegate void ClientReceived(Socket socket, byte[] data);

    /// <summary>
    /// 客户端断开连接后，触发的事件模板委托
    /// </summary>
    /// <param name="socket"></param>
    public delegate void ClientClosed(Socket socket);
}
