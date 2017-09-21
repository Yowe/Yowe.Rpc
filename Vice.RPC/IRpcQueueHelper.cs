using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Vice.RPC
{
    public interface IRpcQueueHelper
    {
        void CreateQueues(string connectionString, Action<IModel> createExchangesAndQueues);
        string TryGetValidConnectionString(string preferConnectionString);
    }

    
}
