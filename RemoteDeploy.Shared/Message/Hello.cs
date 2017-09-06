using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDeploy.Shared.Message
{
    public class Hello
    {
        public Hello(string message)
        {
            Message = message;
        }

        public string Message { get; private set; }
    }
}
