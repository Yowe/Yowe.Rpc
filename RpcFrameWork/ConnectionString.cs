using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    /// <summary>
    /// 连接RabbitMQ实例的字符串
    /// 须按照以下格式
    /// host=192.168.1.1;port=5672;virtualHost=MyVirtualHost;username=MyUsername;password=MyPassword
    /// </summary>
    public class ConnectionString
    {
        private readonly IDictionary<string, string> _parametersDictionary = new Dictionary<string, string>();

        public ConnectionString(string connectionStringValue)
        {
            if (string.IsNullOrEmpty(connectionStringValue))
            {
                throw new ArgumentNullException(nameof(connectionStringValue));
            }

            var KeyValuePairs = connectionStringValue.Split(';');
            foreach (var keyvaluepair in KeyValuePairs)
            {
                if (string.IsNullOrWhiteSpace(keyvaluepair))
                    continue;

                var keyValueParts = keyvaluepair.Split('=');
                if (KeyValuePairs.Length != 2)
                {
                    throw new Exception("连接字符串必须是键值对:key=value");
                }

                _parametersDictionary.Add(keyValueParts[0], keyValueParts[1]);
            }


            Port = int.Parse(GetValue("port", "5672"));
            Host = GetValue("host", "localhost");
            VirtualHost = GetValue("virtualhost", "/");
            UserName = GetValue("username", "guest");
            Password = GetValue("password", "guest");

            if (Host.Contains(":"))
            {
                var index = Host.IndexOf(":", StringComparison.Ordinal);
                Port = int.Parse(Host.Substring(index + 1));
                Host = Host.Substring(0, index);
            }
        }

        /// <summary>
        /// 获取指定键对应的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetValue(string key)
        {
            if(key==null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var lowerKey = key.ToLower();
            if (!_parametersDictionary.ContainsKey(key))
            {
                throw new Exception(string.Format("{0} key 不存在.",key));
            }
            return _parametersDictionary[key];
        }
        public string GetValue(string key, string defaultValue)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            var lowerKey = key.ToLower();
            return _parametersDictionary.ContainsKey(lowerKey) && !string.IsNullOrEmpty(_parametersDictionary[lowerKey])
                ? _parametersDictionary[lowerKey]
                : defaultValue;
        }

        /// <summary>
        /// rabbitMQ 端口号
        /// </summary>
        public int Port { get; set; }
        /// <summary>
        /// rabbitMQ 主机
        /// </summary>
        public string Host { get; set; }
        /// <summary>
        /// rabbitMQ 虚拟主机
        /// </summary>
        public string VirtualHost { get; set; }
        /// <summary>
        /// 用户名 default is guest
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// The rabbitMQ 密码, default is guest
        /// </summary>
        public string Password { get; private set; }
    }
}
