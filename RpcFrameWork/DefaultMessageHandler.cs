using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork.Internal
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

    }
}
