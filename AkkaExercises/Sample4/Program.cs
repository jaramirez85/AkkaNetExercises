using System;
using System.Diagnostics;
using Akka.Actor;
using AkkaExercises.Sample4.Actors;
using AkkaExercises.Sample4.Messages;

namespace AkkaExercises.Sample4
{
    class RoutersDemo
    {
        private static ActorSystem ActorSystem;

        static void Main(string[] args)
        {
            ActorSystem = ActorSystem.Create("PaymentProcessing");

            IActorRef jobCoordinator = ActorSystem.ActorOf<JobCoordinatorActor>("JobCoordinator");

            //With RoundRobinGroup
            ActorSystem.ActorOf(Props.Create<PaymentWorkerActor>(), "PaymentWorker1");
            ActorSystem.ActorOf(Props.Create<PaymentWorkerActor>(), "PaymentWorker2");
            ActorSystem.ActorOf(Props.Create<PaymentWorkerActor>(), "PaymentWorker3");

            var jobTime = Stopwatch.StartNew();

            jobCoordinator.Tell(new ProcessFileMessage("Sample4/file1.csv"));

            ActorSystem.RegisterOnTermination(() =>
            {
                jobTime.Stop();
                Console.WriteLine("Job complete in {0}ms ", jobTime.ElapsedMilliseconds);

            });

            Console.ReadLine();
        }

    }
}
