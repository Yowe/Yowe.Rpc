using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;

namespace Vice.RPC
{
    [ExcludeFromCodeCoverage]
    internal class Helper : IRpcQueueHelper
    {
        public void CreateQueues(string connectionString, Action<RabbitMQ.Client.IModel> createExchangesAndQueues)
        {
            var clusterConnections = connectionString.Split(new[] { '|' }, StringSplitOptions.RemoveEmptyEntries);
            if (clusterConnections.Length > 1)
            {
                
            }
        }

        public string TryGetValidConnectionString(string preferConnectionString)
        {
            throw new NotImplementedException();
        }
    }
}
