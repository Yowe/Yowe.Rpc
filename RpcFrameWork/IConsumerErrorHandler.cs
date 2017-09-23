using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    /// <summary>
    /// 当出现错误处理消息时，提供一个错误处理策略
    /// </summary>
    public interface IConsumerErrorHandler
    {
        void HandleError(BasicDeliverEventArgs deliverEventArgs, Exception exception);
    }
}
