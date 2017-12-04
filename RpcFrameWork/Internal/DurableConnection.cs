using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using System.Threading;

namespace RpcFrameWork.Internal
{
    /// <summary>
    /// RabbitMQ服务器的连接,并在连接丢失时重试
    /// </summary>
    public class DurableConnection : IDurableConnection
    {

        protected readonly IRetryPolicy _retryPolicy;
        protected readonly IRabbitWatcher _watcher;
        protected Action _unsubscribeEvents = () => { };
        /// <summary>
        /// 如果连接建立，将被触发的事件
        /// </summary>
        public event Action Connected;
        /// <summary>
        /// 断开连接触发事件
        /// </summary>
        public event Action Disconnected;

        public DurableConnection(IRetryPolicy retryPolicy, IRabbitWatcher watcher)
        {
            if (retryPolicy == null)
            {
                throw new ArgumentNullException(nameof(retryPolicy));
            }
            if (watcher == null)
            {
                throw new ArgumentNullException(nameof(watcher));
            }

            _retryPolicy = retryPolicy;
            _watcher = watcher;
        }

        public DurableConnection(IRetryPolicy retryPolicy, IRabbitWatcher watcher, ConnectionFactory connectionFactory) : this(retryPolicy, watcher)
        {
            if (connectionFactory == null)
            {
                throw new ArgumentNullException(nameof(connectionFactory));
            }

            _connectionFactory = ManagedConnectionFactory.CreateFromConnectionFactory(connectionFactory);

            ConnectionEstablished hander = (endpoint, virtualHost) =>
            {
                if (_connectionFactory.Endpoint + _connectionFactory.VirtualHost == endpoint + virtualHost)
                {
                    //NOTE: 当集群中的服务器的一个新连接被建立时，触发事件连接事件
                    FireConnectedEvent();
                }
            };

            ManagedConnectionFactory.ConnectionEstablished += hander;
            _unsubscribeEvents = () =>
            {
                ManagedConnectionFactory.ConnectionEstablished -= hander;
            };

        }

        /// <summary>
        /// 尝试连接到rabbitmq服务器，如果连接失败，则重试。
        /// </summary>
        public void Connect()
        {
            Monitor.Enter(ManagedConnectionFactory.SyncConnection);

            try
            {
                if (IsConnected || _retryPolicy.IsWaiting)
                {
                    return;
                }

                _watcher.DebugFormat("试图连接到端点:{0}", ConnectionFactory.Endpoint);
                var newConnection = ConnectionFactory.CreateConnection();
                newConnection.ConnectionShutdown += NewConnection_ConnectionShutdown;

                _retryPolicy.Reset();
                _watcher.InfoFormat("连接到RabbitMQ:Broker: {0}, VHost: {1}", ConnectionFactory.Endpoint, ConnectionFactory.HostName);
            }
            catch (ConnectFailureException connectFailureException)
            {
                HandleConnectionException(connectFailureException);
            }
            catch (BrokerUnreachableException brokerUnreachableException)
            {
                HandleConnectionException(brokerUnreachableException);
            }
            finally
            {
                Monitor.Exit(ManagedConnectionFactory.SyncConnection);
            }
        }

        private void HandleConnectionException(Exception ex)
        {
            _watcher.ErrorFormat("连接主机失败：{0}，Host:{1},尝试 {2} 秒后重试.ExceptionMessage:{3}",
                   ConnectionFactory.HostName,
                    ConnectionFactory.VirtualHost,
                    _retryPolicy.DelayTime,
                    ex.Message);

            _retryPolicy.WaitForNextRetry(Connect);
        }

        private void NewConnection_ConnectionShutdown(IConnection connection, ShutdownEventArgs reason)
        {
            FireDisconnectedEvent();
            _watcher.WarnFormat("断开RabbitMQ代理 '{0}': {1}", connection.Endpoint, reason != null ? reason.ReplyText : "");
            if (reason != null && reason.ReplyText != "Connection disposed by application" &&
                reason.ReplyText != "")
            {
                _retryPolicy.WaitForNextRetry(Connect);
            }
        }

        public IModel CreateChannel()
        {
            if (!IsConnected)
            {
                Connect();
            }

            if (!IsConnected)
            {
                throw new BrokerUnreachableException(new Exception("Cannot connect to Rabbit server."));
            }

            var connection = ManagedConnectionFactory.SharedConnections[ConnectionFactory.Endpoint + ConnectionFactory.VirtualHost];
            var channel = connection.CreateModel();
            return channel;
        }


        public void Dispose()
        {
            _unsubscribeEvents();
        }

        protected void FireConnectedEvent()
        {
            if (Connected != null)
            {
                Connected.Invoke();
            }
        }

        protected void FireDisconnectedEvent()
        {
            Disconnected?.Invoke();
        }

        #region ------------属性-----------
        public string HostName
        {
            get { return _connectionFactory.HostName; }
        }

        public bool IsConnected
        {
            get
            {
                var key = ConnectionFactory.Endpoint + ConnectionFactory.VirtualHost;
                return true;
            }
        }

        public string UserName
        {
            get { return ConnectionFactory.UserName; }
        }

        public string VirtualHost
        {
            get { return ConnectionFactory.VirtualHost; }
        }

        private readonly ConnectionFactory _connectionFactory;
        protected virtual ConnectionFactory ConnectionFactory
        {
            get { return _connectionFactory; }
        }
        #endregion
    }
}
