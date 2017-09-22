using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{

    public delegate void MessageHandlingEvent(BasicDeliverEventArgs eventArgs);

    public delegate void MessageWasNotHandledEvent(BasicDeliverEventArgs eventArgs);

    public interface IMessageHandler
    {
        void HandleMessage(BasicDeliverEventArgs eventArg);
        void HandleError(BasicDeliverEventArgs eventArg, Exception excepton);
        /// <summary>
        /// 消息处理完毕回调
        /// </summary>
        event MessageHandlingEvent HandlingComplete;
        /// <summary>
        /// 消息处理失败后回调
        /// </summary>
        event MessageWasNotHandledEvent MessageWasNotHandled;
    }
}
