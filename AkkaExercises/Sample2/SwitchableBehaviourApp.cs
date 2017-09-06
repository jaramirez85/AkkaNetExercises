using Akka.Actor;
using System;

namespace AkkaExercises.Sample2
{
    class SwitchableBehaviourApp
    {
        static void Main(string[] args)
        {
            var system = ActorSystem.Create("MySystemBehavior");
            var behaviour = system.ActorOf<SwitchableBehaviourActor>("behaviour");
            behaviour.Tell("hi1");
            behaviour.Tell("hi2");
            behaviour.Tell("hi3");
            Console.ReadKey();
            system.Terminate();
        }
    }
}
