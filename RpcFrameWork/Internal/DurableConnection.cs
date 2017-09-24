using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;

namespace RpcFrameWork.Internal
{
    public class DurableConnection : IDurableConnection
    {


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

        public event Action Connected;
        public event Action Disconnected;

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
    }
}
