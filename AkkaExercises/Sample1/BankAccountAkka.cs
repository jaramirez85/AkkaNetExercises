using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AkkaExercises.Sample1
{
    class BankAccountAkka
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("BankSystem");

            var accountActor = system.ActorOf<BankAccountActor>("account");

            var tasks = new List<Task>();
            for (int i = 0; i < 20; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                        accountActor.Tell(1);
                }));
            }
            Task.WaitAll(tasks.ToArray());

            //var balance = accountActor.Ask<int>("saldo");
            //Console.WriteLine(string.Concat("Expected value 20000", ", actual value ", balance.Result));
            Console.WriteLine(string.Concat("Expected value 20000", ", actual value "));
            accountActor.Tell("balance");


            Console.ReadKey();
            //accountActor.Tell("stop");
            //accountActor.Tell(PoisonPill.Instance);
            Console.ReadKey();
            system.Terminate();
            Console.ReadKey();
        }
    }

    public class BankAccountActor : ReceiveActor
    {
        private int _balance;

        public BankAccountActor()
        {
            Receive<int>(i => _balance += i);
            Receive<string>(s => s.Equals("balance"), _ =>
            {
                Console.WriteLine($"$$-->{_balance}");
                //Sender.Tell(_balance);
            });
            Receive<string>(s => s.Equals("stop"), _ =>
            {
                Context.Stop(Self);
            });
            ReceiveAny(o => Unhandled(o));
        }

        protected override void PreStart()
        {
            Console.WriteLine("PreStart...");
        }

        protected override void PostStop()
        {
            Console.WriteLine("PostStop...");
        }
    }
}
