using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;

namespace RpcFrameWork.Internal
{
    public class ConsumerErrorHandler : IConsumerErrorHandler, IObserver<ISerializer>
    {

        private readonly string _errorQueue;
        private readonly string _errorExchange;
        private readonly IDurableConnection _durableConnection;
        private readonly IRabbitWatcher _watcher;
        private readonly object _channelGate = new object();

        private ISerializer _serializer;
        private bool _errorQueueDeclared;
        private bool _errorQueueBound;

        public ConsumerErrorHandler(IDurableConnection durableConnection, ISerializer serializer, IRabbitWatcher watcher)
        {
            if (durableConnection == null)
            {
                throw new ArgumentNullException(nameof(durableConnection));
            }
            if (serializer == null)
            {
                throw new ArgumentNullException(nameof(serializer));
            }
            if (watcher == null)
            {
                throw new ArgumentNullException(nameof(watcher));
            }

            _durableConnection = durableConnection;
            _watcher = watcher;
            _serializer = serializer;

            _errorQueue = Global.DefaultErrorQueueName ?? "Burrow.Queue.Error";
            _errorExchange = Global.DefaultErrorExchangeName ?? "Burrow.Exchange.Error";
        }

        private void DeclareErrorQueue(IModel model)
        {
            if (!_errorQueueDeclared)
            {
                model.QueueDeclare(_errorQueue,
                   durable: true,
                   exclusive: false,
                   autoDelete: false,
                   arguments: null);

                _errorQueueDeclared = true;
            }
        }

        public void HandleError(BasicDeliverEventArgs deliverEventArgs, Exception exception)
        {
            throw new NotImplementedException();
        }

        public void OnCompleted()
        {
            throw new NotImplementedException();
        }

        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        public void OnNext(ISerializer value)
        {
            throw new NotImplementedException();
        }
    }
}
