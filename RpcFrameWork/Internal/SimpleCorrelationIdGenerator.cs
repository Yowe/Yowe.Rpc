using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork.Internal
{
    internal class SimpleCorrelationIdGenerator : ICorrelationIdGenerator
    {
        public string GenerateCorrelationId()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
