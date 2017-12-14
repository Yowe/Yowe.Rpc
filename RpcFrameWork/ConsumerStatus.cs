using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    /// <summary>
    /// 消费者状态
    /// </summary>
    public enum ConsumerStatus
    {
        /// <summary>
        /// Active
        /// </summary>
        Active,
        /// <summary>
        /// Waiting for running tasks to finish 
        /// </summary>
        Disposing,
        /// <summary>
        /// Disposed
        /// </summary>
        Disposed
    }
}
