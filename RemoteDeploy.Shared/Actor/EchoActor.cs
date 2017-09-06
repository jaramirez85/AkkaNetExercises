using Akka.Actor;
using RemoteDeploy.Shared.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDeploy.Shared.Actor
{
    public class EchoActor : ReceiveActor
    {

        public EchoActor()
        {
            Receive<Hello>(hello =>
            {
                Console.WriteLine($"[{Sender}]: {hello.Message}");
                Sender.Tell(hello);
            });
        }

    }
}
