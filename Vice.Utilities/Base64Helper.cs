using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vice.Utilities
{
    public class Base64Helper
    {
        public static string EncodeBase64String(string str)
        {
            byte[] b = System.Text.Encoding.Default.GetBytes(str);
            string result= Convert.ToBase64String(b);
            return result;
        }

        public static string DecodeBase64String(string str)
        {
            byte[] c = Convert.FromBase64String(str);
            string result = System.Text.Encoding.Default.GetString(c);
            return result;
        }
    }
}
