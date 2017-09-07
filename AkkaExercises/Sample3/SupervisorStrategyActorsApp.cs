using Akka.Actor;
using System;

namespace AkkaExercises.Sample3
{
    public class SupervisorStrategyActorsApp 
    {
        public static void Main(string[] args)
        {
            Console.Title = "SupervisorStrategyActorsApp";
            using (var system = ActorSystem.Create("SupervisorStrategySystem"))
            {
                system.ActorOf(FatherActor.Props, "Father");
                Console.ReadKey();
            }

            Console.ReadKey();

        }
    }
}
