﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Diagnostics.CodeAnalysis;

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
        /// <summary>
        /// 是否已经绑定错误消息队列
        /// </summary>
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

        /// <summary>
        /// 初始化消息队列
        /// </summary>
        /// <param name="model"></param>
        private void InitializeErrorExchangeAndQueue(IModel model)
        {
            DeclareErrorQueue(model);
            DeclareErrorExchangeAndBindToErrorQueue(model);
        }

        /// <summary>
        /// 声明交换器并绑定错误信息队列
        /// </summary>
        /// <param name="model"></param>
        private void DeclareErrorExchangeAndBindToErrorQueue(IModel model)
        {
            if (!_errorQueueBound)
            {
                model.ExchangeDeclare(_errorExchange, ExchangeType.Direct, durable: true);
                model.QueueBind(_errorQueue, _errorQueue, string.Empty);

                _errorQueueBound = true;
            }
        }

        /// <summary>
        /// 声明错误信息队列
        /// </summary>
        /// <param name="model"></param>
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

        private string CreateConnectionCheckMessage(IDurableConnection durableConnection)
        {
            return
                "Please check connection string and that the RabbitMQ Service is running at the specified endpoint.\n" +
                $"\tHostname: '{durableConnection.HostName}'\n" +
                $"\tVirtualHost: '{durableConnection.VirtualHost}'\n" +
                $"\tUserName: '{durableConnection.UserName}'\n" +
                "Failed to write error message to error queue";
        }

        public void Dispose()
        {
        }

        public void HandleError(BasicDeliverEventArgs deliverEventArgs, Exception exception)
        {
            try
            {

            }
            catch (ConnectFailureException)
            {
                _watcher.ErrorFormat(("ConsumerErrorHandler: cannot connect to Broker.\n" + CreateConnectionCheckMessage(_durableConnection)));
            }
            catch (OperationInterruptedException interruptedException)
            {
                // thrown if the broker connection is broken during declare or publish.
                _watcher.ErrorFormat(
                    "ConsumerErrorHandler: Broker connection was closed while attempting to publish Error message.\n" +
                    $"Message was: '{interruptedException.Message}'\n" +
                    CreateConnectionCheckMessage(_durableConnection));
            }
            catch (Exception unexpecctedException)
            {
                _watcher.ErrorFormat("ConsumerErrorHandler: Failed to publish error message\nException is:\n" + unexpecctedException);
            }
        }

        [ExcludeFromCodeCoverage]
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
        [ExcludeFromCodeCoverage]
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }
        [ExcludeFromCodeCoverage]
        public void OnNext(ISerializer value)
        {
            throw new NotImplementedException();
        }
    }
}
