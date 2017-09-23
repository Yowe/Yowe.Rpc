using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    public interface IDurableConnection:IDisposable
    {
        /// <summary>
        /// 当连接到rabbitmq服务器时，将触发一个事件
        /// </summary>
        event Action Connected;
        /// <summary>
        /// 当断开连接到rabbitmq服务器时，将触发一个事件
        /// </summary>
        event Action Disconnected;
        /// <summary>
        /// 确定是否与rabbitmq服务器有连接
        /// </summary>
        bool IsConnected { get; }
        /// <summary>
        /// 获取当前连接的主机名
        /// </summary>
        string HostName { get; }
        /// <summary>
        /// 获取当前连接的虚拟主机名
        /// </summary>
        string VirtualHost { get; }
        /// <summary>
        /// 获取当前连接的用户名
        /// </summary>
        string UserName { get; }
        /// <summary>
        /// 连接到rabbitmq服务器
        /// </summary>
        void Connect();
        /// <summary>
        /// 创建一个rabbitmq通道，该通道可以用于发布/订阅
        /// </summary>
        /// <returns></returns>
        IModel CreateChannel();
    }
}
