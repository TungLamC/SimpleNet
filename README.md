# SimpleNet

***

一个简易的游戏服务器底层通信库，使用C#的 BeginAccept / EndAccpet 以及 BeginReceive / EndReceive 实现异步接收

# 使用指南

***

双击SimpleNet.sln使用visual studio打开该项目，并右键生成，将生成的SimpleNet.dll导入到项目中即可使用

# 使用步骤

***

### 引入命名空间

```c#
using SimpleNet;
```

### 创建Listener对象

```c#
// 构造方法的两个参数分别为要监听的IP地址和端口号
Listener listener = new Listener("127.0.0.1", 8888);
```

### 事件讲解

SImpleNet的关键事件有三种：客户端连接，接受到客户端的数据，客户端断开，三种事件所使用的委托模板如下：

```c#
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
```

### 为通信库添加事件

```c#
listener.AddAccepted(ClientAccepted);
listener.AddReceived(ClientReceived);
listener.AddClosed(ClientClosed);
```

在添加关键事件时要遵循上述的委托模板

### 开始运作

```c#
listener.Start();
```


### 事件调用顺序

当接收到一个客户端的连接请求时，通过AddAccepted添加的方法将依次执行，参数为与该客户端通信所使用的socket

当接收到一个客户端发送过来的数据时，通过AddReceived添加的方法将依次执行，参数分别为与该客户端通信所使用的socket和存储接收数据的字节数组

当一个客户端断开连接时，通过AddClosed添加的方法将依次执行，参数为与该客户端通信的socket

