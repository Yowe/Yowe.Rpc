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
        internal protected Subscription(IModel model) : this()
        {
            SetChannel(model);
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
        /// <summary>
        /// 通过发送标签来发送消息
        /// </summary>
        /// <param name="deliveryTag"></param>
        public void Ack(ulong deliveryTag)
        {
            TryAck(_channel, deliveryTag, false);
        }

        public void Nack(IEnumerable<ulong> deliveryTags, bool requeue)
        {

        }

        private void TryAck(IModel channel, ulong deliveryTag, bool multiple, IRabbitWatcher watcher = null)
        {
            TryAckOrNack(ConsumerTag, true, channel, deliveryTag,, multiple, watcher);
        }
        private void TryNack(IModel channel, ulong deliveryTag, bool multiple, bool requeue, IRabbitWatcher watcher = null)
        {
            TryAckOrNack(ConsumerTag, false, channel, deliveryTag, multiple, requeue, watcher);
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

        private const string FailedToAckMessage = "Basic ack/nack failed because chanel was closed with message {0}. Message remains on RabbitMQ and will be retried.";
       
        internal static void TryAckOrNack(string consumerTag, bool ack, IModel channel, ulong deliveryTag, bool multiple, bool requeue, IRabbitWatcher watcher = null)
        {
            try
            {
                if (channel == null)
                {
                    (watcher ?? Global.DefaultWatcher).WarnFormat("Trying ack/nack msg but the Channel is null, will not do anything");
                }
                else if (!channel.IsOpen)
                {
                    (watcher ?? Global.DefaultWatcher).WarnFormat("Trying ack/nack msg but the Channel is not open, will not do anything");
                }
                else
                {
                    if (ack)
                    {
                        channel.BasicAck(deliveryTag, multiple);
                    }
                    else
                    {
                        channel.BasicNack(deliveryTag, multiple, requeue);
                    }

                    lock (OutstandingDeliveryTags)
                    {
                        if (OutstandingDeliveryTags.ContainsKey(consumerTag))
                        {
                            if (deliveryTag == 0)
                            {
                                OutstandingDeliveryTags[consumerTag].Clear();
                            }
                            else if (multiple)
                            {
                                OutstandingDeliveryTags[consumerTag].RemoveAll(x => x <= deliveryTag);
                            }
                            else
                            {
                                OutstandingDeliveryTags[consumerTag].Remove(deliveryTag);
                            }
                        }
                    }
                }
            }
            catch (AlreadyClosedException alreadyClosedException)
            {
                (watcher ?? Global.DefaultWatcher).WarnFormat(FailedToAckMessage, alreadyClosedException.Message);
            }
            catch (IOException ioException)
            {
                (watcher ?? Global.DefaultWatcher).WarnFormat(FailedToAckMessage, ioException.Message);
            }
        }

     
    }
}
