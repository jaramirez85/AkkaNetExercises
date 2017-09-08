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
                IActorRef pongActor = actorSystem.ActorOf(Props.Create<PongActor>(),"pong");
                IActorRef pingActor = actorSystem.ActorOf(Props.Create(() => new PingActor(pongActor)), "ping");

                pingActor.Tell(new Pong());
                Console.ReadLine();
            }

        }

    }

    public class PingActor : ReceiveActor
    {

        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly IActorRef _pong;
        public PingActor(IActorRef pong)
        {
            _pong = pong;
            Receive<Pong>(_ => {
                _logger.Info("PING >>>>>");
                Thread.Sleep(TimeSpan.FromMilliseconds(300));
                _pong.Tell(new Ping(), Self);
            });
        }
    }

    public class PongActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        public PongActor()
        {
            Receive<Ping>(_ => {
                _logger.Info("<<<<< PONG");
                Thread.Sleep(TimeSpan.FromMilliseconds(300));
                Sender.Tell(new Pong());
            });
        }
    }


    #region messages
    public class Ping{}
    public class Pong {}
    #endregion

}
