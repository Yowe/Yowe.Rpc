﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    public class ConstantRouteFinder : IRouteFinder
    {
        private readonly string _exchangeName;
        private readonly string _queueName;
        private readonly string _routingKey;

        public ConstantRouteFinder(string exchangeName, string queueName, string routingKey)
        {
            _exchangeName = exchangeName;
            _queueName = queueName;
            _routingKey = routingKey;
        }

        public string FindExchangeName<T>()
        {
            return _exchangeName;
        }

        public string FindQueueName<T>(string subscriptionName)
        {
            return _queueName;
        }

        public string FindRoutingKey<T>()
        {
            return _routingKey;
        }
    }
}
