using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace RpcFrameWork.Internal
{
    public class BinarySerializer : ISerializer
    {
        public T Deserialize<T>(byte[] bytes)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException(nameof(bytes));
            }

            var formatter = new BinaryFormatter();
            using (var stream = new MemoryStream(bytes))
            {
                return (T)formatter.Deserialize(stream);
            }
        }

        public byte[] Serialize<T>(T message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            var formatter = new BinaryFormatter();
            byte[] messageBody;

            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, message);
                messageBody = stream.GetBuffer();
            }
            return messageBody;
        }
    }
}
