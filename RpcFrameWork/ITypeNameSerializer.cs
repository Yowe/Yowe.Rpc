using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RpcFrameWork
{
    public interface ITypeNameSerializer
    {
        string Serialize(Type type);
    }
}
