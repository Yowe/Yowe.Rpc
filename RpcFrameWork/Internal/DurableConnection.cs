using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace RpcFrameWork.Internal
{
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
        public event Action DisConnected;
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
            get  {  return ConnectionFactory.UserName; }
        }

        public string VirtualHost
        {
            get{ return ConnectionFactory.VirtualHost;  }
        }

        private readonly ConnectionFactory _connectionFactory;
        protected virtual ConnectionFactory ConnectionFactory
        {
            get { return _connectionFactory; }
        }

        public void Connect()
        {
            throw new NotImplementedException();
        }

        public IModel CreateChannel()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        protected void FireConnectedEvent()
        {
            if(Connected!=null)
            {
                Connected.Invoke();
            }
        }
    }
}
