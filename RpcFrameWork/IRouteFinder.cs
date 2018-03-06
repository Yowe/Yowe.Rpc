using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    public interface IRouteFinder
    {
        string FindExchangeName<T>();
        string FindRoutingKey<T>();
        string FindQueueName<T>(string subscriptionName);
    }
}
