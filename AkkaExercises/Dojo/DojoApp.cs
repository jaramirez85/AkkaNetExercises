
using Akka.Actor;
using Akka.Event;
using System;
using System.Threading;

namespace AkkaExercises.Dojo
{
    public class DojoApp
    {
        static void Main(string[] args)
        {
            using (var actorSystem = ActorSystem.Create("PingPong"))
            {
                actorSystem.ActorOf(Props.Create<PongActor>(), "pong1");
                actorSystem.ActorOf(Props.Create<PongActor>(), "pong2");
                actorSystem.ActorOf(Props.Create<PingActor>(), "ping");
                Console.ReadLine();
            }
            Console.ReadLine();
        }

    }

    public class PingActor : ReceiveActor
    {
        private int _pongsReceived;
        private readonly ILoggingAdapter _logger = Context.GetLogger();

        public PingActor()
        {
            Context.ActorSelection("/user/pong*").Tell(new Ping());

            Receive<Pong>(_ => {
                _pongsReceived ++;
                _logger.Info($"{_pongsReceived} PING >>>>>");
                Thread.Sleep(TimeSpan.FromMilliseconds(300));
                Sender.Tell(new Ping());
            });
        }
    }

    public class PongActor : ReceiveActor
    {
        private int _pingsReceived;
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public PongActor()
        {
            Receive<Ping>(_ => {
                _pingsReceived++;
                _logger.Warning($"<<<<< PONG {_pingsReceived}");
                Thread.Sleep(TimeSpan.FromMilliseconds(300));
                Sender.Tell(new Pong());
            });
        }
    }


    #region messages
    public class Ping { }
    public class Pong { }
    #endregion

}
