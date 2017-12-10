using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    public class MessageDeliverEventArgs
    {
        public string SubscriptionName
        {
            get;set;
        }

        public string CounsumerTag
        {
            get;set;
        }

        public ulong DeliveryTag
        {
            get; set;
        }

        public string MessagePriorty
        {
            get;set;
        }
    }
}
