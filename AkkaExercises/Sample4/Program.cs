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
            var jobTime = Stopwatch.StartNew();
            jobCoordinator.Tell(new ProcessFileMessage("Sample4/file1.csv"));


            ActorSystem.WhenTerminated.Wait();

            jobTime.Stop();
            Console.WriteLine("Job complete in {0}ms ", jobTime.ElapsedMilliseconds);

            Console.ReadLine();
        }

    }
}
