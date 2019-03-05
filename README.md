# SimpleNet

***

一个简易的游戏服务器底层通信库，使用C#的 BeginAccept / EndAccpet 以及 BeginReceive / EndReceive 实现异步接收

# 使用指南

***

双击SimpleNet.sln使用visual studio打开该项目，并右键生成，将生成的SimpleNet.dll导入到项目中即可使用

## 使用步骤

### 引入命名空间

```c#
using SimpleNet;
```

### 创建Listener对象

```c#
// 构造方法的两个参数分别为要监听的IP地址和端口号
Listener listener = new Listener("127.0.0.1", 8888);
```

### 添加通信事件

```
listener.AddAccepted(ClientAccepted);
listener.AddReceived(ClientReceived);
listener.AddClosed(ClientClosed);
```



