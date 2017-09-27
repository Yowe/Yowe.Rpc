﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    public static class Global
    {
        /// <summary>
        /// 错误消息将被发布到的错误队列名称。
        /// <para>你不需要创建这个队列，当出现错误时，库会创建它</para>
        /// </summary>
        public static string DefaultErrorQueueName = "Vice.Queue.Error";

        /// <summary>
        /// 错误消息将被发布到错误消息的名称
        /// <para>You don't have to create this exchange, the library will create it when there is an error</para>
        /// </summary>
        public static string DefaultErrorExchangeName = "Vice.Exchange.Error";
    }
}