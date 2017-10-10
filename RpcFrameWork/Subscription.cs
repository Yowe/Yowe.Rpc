using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    /// <summary>
    /// 对rabbitmq.client的引用的包装器
    /// 从这里你可以发出一个信息或者取消消费者的信息
    /// </summary>
    public class Subscription
    {
        internal const string CloseApplication = "Closed by application";
        internal static ConcurrentDictionary<string, List<ulong>> OutstandingDeliveryTags = new ConcurrentDictionary<string, List<ulong>>();

        private IModel _channel;
        /// <summary>
        /// 队列名称
        /// </summary>
        public string QueueName { get; set; }
        /// <summary>
        /// 订阅者的名称
        /// </summary>
        public string SubscriptionName { get; set; }
        /// <summary>
        /// 消费者标签
        /// </summary>
        public string ConsumerTag { get; set; }

        internal protected Subscription()
        {
        }

        public void SetChannel(IModel channel)
        {
            if (channel == null)
            {
                throw new ArgumentNullException(nameof(channel));
            }
            _channel = channel;
        }

        /// <summary>
        /// 取消订阅
        /// </summary>
        public void Cancel()
        {
            TryCancel(x => x.BasicCancel(ConsumerTag), _channel, Global.DefaultWatcher);
        }



        internal void TryCancel(Action<IModel> action, IModel channel, IRabbitWatcher watcher)
        {
            const string failedMessage = "Action failed because chanel was closed with message {0}. ";
            try
            {
                watcher.InfoFormat("Cancelling subscription {0} on queue {1}", SubscriptionName, QueueName);
                action(channel);
                watcher.InfoFormat("Subscription {0}  on queue {1} cancelled", SubscriptionName, QueueName);
            }
            catch (AlreadyClosedException alreadyClosedException)
            {
                watcher.WarnFormat(failedMessage, alreadyClosedException.Message);
            }
            catch (IOException ioException)
            {
                watcher.WarnFormat(failedMessage, ioException.Message);
            }
        }
    }
}
