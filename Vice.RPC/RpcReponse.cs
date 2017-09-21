using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Vice.RPC
{
    public class RpcReponse
    {
        /// <summary>
        /// 服务返回异常信息
        /// </summary>
        public Exception Exception { get; set; }
        public Guid RequestId { get; set; }
        /// <summary>
        /// RPC 调用函数返回值
        /// </summary>
        public object ReturnValue { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public Dictionary<string, object> ChangedParams { get; set; }
    }
}
