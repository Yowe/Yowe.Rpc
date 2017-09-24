using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    /// <summary>
    /// 实现此接口以定义策略，在尝试失败后应该如何创建连接
    /// </summary>
    public interface IRetryPolicy
    {
        /// <summary>
        /// 等待时间
        /// </summary>
        int DelayTime { get; }

        /// <summary>
        /// 确定它是否正在等待建立连接
        /// </summary>
        bool IsWaiting { get; }

        /// <summary>
        /// 等待并执行重试操作的async方法
        /// </summary>
        /// <param name="retryingAction"></param>
        void WaitForNextRetry(Action retryingAction);

        /// <summary>
        /// 一旦建立连接，就重新设置策略
        /// </summary>
        void Reset();
    }
}
