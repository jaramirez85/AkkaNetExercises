using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AkkaExercises.Sample1
{
    class HelloWordAkka
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("MySystem");
            //var greeter = system.ActorOf<GreetingActor>("greeter");
            var greeter = system.ActorOf(Props.Create<GreetingActor>());

            //var range = Enumerable.Range(1, 100).AsParallel();
            //foreach(var i in range)
            greeter.Tell(new Greet("Hello World"));
            Console.ReadKey();
            system.Terminate();
        }
    }

    public class GreetingActor : UntypedActor
    {
        private int _count;

        protected override void OnReceive(object message)
        {
            if(message is Greet)
            {
                _count++;
                var greet = message as Greet;
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Greeting {greet.Who}: {_count}");
            }
        }
        /* public GreetingActor()
        {
            Receive<Greet>(greet =>
            {

                _count++;
                Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} Greeting {greet.Who}: {_count}");
            });
        }*/


    }


    #region Messages
    public class Greet
    {
        public Greet(string who)
        {
            Who = who;
        }

        public string Who { get; private set; }
    }
#endregion
}
