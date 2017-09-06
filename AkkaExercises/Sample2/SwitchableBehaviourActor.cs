using Akka.Actor;
using System;

namespace AkkaExercises.Sample2
{
    class SwitchableBehaviourActor : ReceiveActor
    {
        public SwitchableBehaviourActor()
        {
            Become(ABehaviour);
        }

        private void ABehaviour()
        {
            Receive<string>(_ => {
                Console.WriteLine("From (A) Behaviour");
                Become(BBehaviour);
            });
        }

        private void BBehaviour()
        {
            Receive<string>(_ => {
                Console.WriteLine("From (B) Behaviour");
                Become(ABehaviour);
            });
        }
    }
}
