using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    public class DefaultMessageHandler<T>:IMessageHandler
    {
        protected readonly string _typeName = Global.DefaultTypeNameSerializer.Serialize(typeof(T));
        protected readonly string _subscriptionName;
        protected readonly IRabbitWatcher _watcher;
        protected readonly Action<T, MessageDeliverEventArgs> _msgHandlingAction;
        protected readonly IConsumerErrorHandler _consumerErrorHandler;
        protected readonly ISerializer _messageSerializer;

        public event MessageHandlingEvent HandlingComplete;
        public event MessageWasNotHandledEvent MessageWasNotHandled;

        public DefaultMessageHandler(string subscriptionName,
            Action<T, MessageDeliverEventArgs> msgHandlingAction,
            IConsumerErrorHandler consumerErrorHandler,
            ISerializer messageSerializer,
            IRabbitWatcher watcher)
        {
            if (msgHandlingAction == null)
                throw new ArgumentNullException(nameof(msgHandlingAction));

            if (consumerErrorHandler == null)
                throw new ArgumentNullException(nameof(consumerErrorHandler));

            if (messageSerializer == null)
                throw new ArgumentNullException(nameof(messageSerializer));

            if (watcher == null)
                throw new ArgumentNullException(nameof(watcher));

            _subscriptionName = subscriptionName;
            _watcher = watcher;
            _consumerErrorHandler = consumerErrorHandler;
            _messageSerializer = messageSerializer;
            _msgHandlingAction = msgHandlingAction;
            _typeName = Global.DefaultTypeNameSerializer.Serialize(typeof(T));
        }
        /// <summary>
        /// 在处理信息之前可以重新此方法
        /// </summary>
        /// <param name="args"></param>
        protected virtual void BeforeHandlingMessage(BasicDeliverEventArgs args)
        {
        }

        /// <summary>
        /// 处理信息之后可以重新此方法
        /// </summary>
        /// <param name="args"></param>
        protected virtual void AfterHandngMessage(BasicDeliverEventArgs args)
        {
        }

        public virtual void HandleError(BasicDeliverEventArgs eventArgs, Exception exception)
        {
            _watcher.ErrorFormat(BuildErrorLogMessage(eventArgs, exception));
            _consumerErrorHandler.HandleError(eventArgs, exception);
        }

        protected virtual string BuildErrorLogMessage(BasicDeliverEventArgs basicDeliverEventArgs, Exception exception)
        {
            var message = Encoding.UTF8.GetString(basicDeliverEventArgs.Body);
            var properties = basicDeliverEventArgs.BasicProperties as RabbitMQ.Client.Impl.BasicProperties;
            var propertiesMessage = new StringBuilder();
            if (properties != null)
                properties.AppendPropertyDebugStringTo(propertiesMessage);

            return "订阅方法引起异常.\n" +
                  $"\tExchange:    '{basicDeliverEventArgs.Exchange}'\n" +
                  $"\tRouting Key: '{basicDeliverEventArgs.RoutingKey}'\n" +
                  $"\tRedelivered: '{basicDeliverEventArgs.Redelivered}'\n" +
                  $" Message:\n{message}\n" +
                  $" BasicProperties:\n{propertiesMessage}\n" +
                  $" Exception:\n{exception}\n";
        }

        public void HandleMessage(BasicDeliverEventArgs eventArgs)
        {
            bool msgHandled = false;
            try
            {
                BeforeHandlingMessage(eventArgs);
                HandleMessage(eventArgs, out msgHandled);
            }
            catch (Exception ex)
            {
                _watcher.Error(ex);
                try
                {
                    HandleError(eventArgs, ex);
                }
                catch (Exception errorHandlingEx)
                {
                    _watcher.ErrorFormat("Failed to handle the exception: {0} because of {1}", ex.Message, errorHandlingEx.StackTrace);
                }
            }
            finally
            {
                CleanUp(eventArgs, msgHandled);
            }
        }

        protected virtual void HandleMessage(BasicDeliverEventArgs eventArgs, out bool msgHandled)
        {
            CheckMessageType(eventArgs.BasicProperties);
            var message = _messageSerializer.Deserialize<T>(eventArgs.Body);

            _msgHandlingAction(message, new MessageDeliverEventArgs
            {
                CounsumerTag = eventArgs.ConsumerTag,
                DeliveryTag = eventArgs.DeliveryTag,
                SubscriptionName = _subscriptionName,
            });
            msgHandled = true;
        }
        /// <summary>
        /// 消息处理完后清理
        /// </summary>
        /// <param name="eventArgs"></param>
        /// <param name="msgHandled"></param>
        internal void CleanUp(BasicDeliverEventArgs eventArgs, bool msgHandled)
        {
            if (!msgHandled && MessageWasNotHandled != null)
            {
                try
                {
                    MessageWasNotHandled(eventArgs);
                }
                catch (Exception exceptionWhenFiringMessageWasNotDeliveredEvent)
                {
                    _watcher.ErrorFormat("触发MessageWasNotHandled事件时出现异常");
                    _watcher.Error(exceptionWhenFiringMessageWasNotDeliveredEvent);
                }
            }

            try
            {
                AfterHandngMessage(eventArgs);
            }
            catch (Exception afterHandlingMessageException)
            {
                _watcher.ErrorFormat("There is an error when trying to call AfterHandlingMessage method");
                _watcher.Error(afterHandlingMessageException);
            }

            if (HandlingComplete != null)
            {
                try
                {
                    HandlingComplete(eventArgs);
                }
                catch (Exception exceptionWhenFiringHandlingCompleteEvent)
                {
                    // Properly should Release pool + DoAck on the BurrowConsumer object
                    _watcher.ErrorFormat("There is an error when trying to fire HandlingComplete event");
                    _watcher.Error(exceptionWhenFiringHandlingCompleteEvent);
                }
            }
        }

        protected void CheckMessageType(IBasicProperties properties)
        {
            if (properties.Type != _typeName)
            {
                _watcher.ErrorFormat("Message type is incorrect. Expected '{0}', but was '{1}'", _typeName, properties.Type);
                throw new Exception($"Message type is incorrect. Expected '{_typeName}', but was '{properties.Type}'");
            }
        }

    }
}
