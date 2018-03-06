using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    public interface IMessageHandlerFactory:IDisposable
    {
        /// <summary>
        /// 创建MessageHandler实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="subscriptionName"></param>
        /// <param name="msgHandingAction"></param>
        /// <returns></returns>
        IMessageHandler Create<T>(string subscriptionName, Action<T, MessageDeliverEventArgs> msgHandingAction);
    }
}
