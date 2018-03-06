using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace RpcFrameWork.Internal
{
    /// <summary>
    /// 当与RabbitMQ服务器建立物理连接时，将触发该事件。
    /// </summary>
    /// <param name="endpoint"></param>
    /// <param name="virtualHost"></param>
    public delegate void ConnectionEstablished(AmqpTcpEndpoint endpoint, string virtualHost);

    public class ManagedConnectionFactory:ConnectionFactory
    {
        internal static readonly object SyncConnection = new object();
        internal static volatile Dictionary<string, IConnection> SharedConnections = new Dictionary<string, IConnection>();

        /// <summary>
        /// 当与RabbitMQ服务器建立物理连接时，将触发该事件。
        /// </summary>
        /// <param name="endpoint"></param>
        /// <param name="virtualHost"></param>
        public static ConnectionEstablished ConnectionEstablished { get; set; }

        public ManagedConnectionFactory()
        {
        }

        public ManagedConnectionFactory(ConnectionString connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            HostName = connectionString.Host;
            Port = connectionString.Port;
            VirtualHost = connectionString.VirtualHost;
            UserName = connectionString.UserName;
            Password = connectionString.Password;

        }

        public ManagedConnectionFactory(ConnectionFactory connectionFactory)
        {
            AuthMechanisms = connectionFactory.AuthMechanisms;
            ClientProperties = connectionFactory.ClientProperties;
            Endpoint = connectionFactory.Endpoint;
            HostName = connectionFactory.HostName;
            Password = connectionFactory.Password;
            Port = connectionFactory.Port;
            Protocol = connectionFactory.Protocol;
            RequestedChannelMax = connectionFactory.RequestedChannelMax;
            RequestedConnectionTimeout = connectionFactory.RequestedConnectionTimeout;
            RequestedFrameMax = connectionFactory.RequestedFrameMax;
            RequestedHeartbeat = connectionFactory.RequestedHeartbeat;
            SocketFactory = connectionFactory.SocketFactory;
            Ssl = connectionFactory.Ssl;
            UserName = connectionFactory.UserName;
            VirtualHost = connectionFactory.VirtualHost;
        }

        public static ManagedConnectionFactory CreateFromConnectionFactory(ConnectionFactory connectionFactory)
        {
            return connectionFactory is ManagedConnectionFactory
                ? connectionFactory as ManagedConnectionFactory
                : new ManagedConnectionFactory(connectionFactory);
        }

        /// <summary>
        /// 自定义创建RabbitMQ的连接
        /// </summary>
        /// <returns></returns>
        public sealed override IConnection CreateConnection()
        {
            var connection = EstablishConnection();
            SaveConnection(connection);
            return connection;

        }
        /// <summary>
        /// 调用基本方法来建立与RabbitMQ的连接
        /// </summary>
        /// <returns></returns>
        [ExcludeFromCodeCoverage]
        internal protected virtual IConnection EstablishConnection()
        {
            return base.CreateConnection();
        }

        /// <summary>
        /// 保存连接对象并触发连接事件
        /// </summary>
        /// <param name="connection"></param>
        private void SaveConnection(IConnection connection)
        {
            if (connection != null && connection.IsOpen)
            {
                var key = Endpoint + VirtualHost;
                SharedConnections[key] = connection;
                connection.ConnectionShutdown += Connection_ConnectionShutdown;

                if(ConnectionEstablished!=null)
                {
                    ConnectionEstablished.Invoke(Endpoint, VirtualHost);
                }
            }
        }
        /// <summary>
        /// 连接断开触发
        /// </summary>
        /// <param name="connection"></param>
        /// <param name="reason"></param>
        private void Connection_ConnectionShutdown(IConnection connection, ShutdownEventArgs reason)
        {
            foreach (var item in SharedConnections)
            {
                if (item.Key == Endpoint + VirtualHost)
                {
                    SharedConnections.Remove(item.Key);
                    break;
                }
            }
        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        internal static void CloseAllConnections()
        {
            SharedConnections.Values.ToList().ForEach(c =>
            {
                c.Close(200, "Connection disposed by application");
                c.Dispose();
            });
            SharedConnections.Clear();
        }
    }
}
