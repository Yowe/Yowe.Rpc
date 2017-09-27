using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork.Internal
{
    public class TypeNameSerializer : ITypeNameSerializer
    {
        public string Serialize(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.FullName.Replace('.', '_')
                + ":"
                + type.Assembly.GetName().Name.Replace('.', '_');
        }
    }
}
