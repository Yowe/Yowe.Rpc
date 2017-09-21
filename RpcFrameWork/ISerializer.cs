using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    public interface ISerializer
    {
        /// <summary>
        /// 序列化字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <returns></returns>
        byte[] Serialize<T>(T message);
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="bytes"></param>
        /// <returns></returns>
        T Deserialize<T>(byte[] bytes);
    }
}
