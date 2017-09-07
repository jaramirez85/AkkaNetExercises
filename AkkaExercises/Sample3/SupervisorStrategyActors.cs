using Akka.Actor;
using Akka.Event;
using System;

namespace AkkaExercises.Sample3
{
    public class FatherActor : ReceiveActor
    {
        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private readonly IActorRef _childActorRef = Context.ActorOf(ChildActor.Props, "Child");

        public FatherActor()
        {
            Receive<Counter>(c => _logger.Info($"received: {c.Count}"));
            ReceiveAny(_ => _logger.Info($"something {_}"));
        }

        protected override SupervisorStrategy SupervisorStrategy()
        {
            return new OneForOneStrategy(
                maxNrOfRetries: 4,
                withinTimeRange: TimeSpan.FromMinutes(1),
                decider: Decider.From(x =>
                {
                    if (x is ArithmeticException)
                        return Directive.Resume;
                    if (x is ArgumentException)
                        return Directive.Restart;
                    if (x is Exception)
                        return Directive.Stop;
                    return Directive.Escalate;
                })
            );
        }

        protected override void PreStart()
        {

            _logger.Warning("FatherActor PreStart.... ");
            Context.System.Scheduler.ScheduleTellRepeatedly(
                TimeSpan.FromMilliseconds(500),
                TimeSpan.FromMilliseconds(500),
                _childActorRef,
                new Next(),
                Self);
        }

        protected override void PostStop()
        {
            _logger.Warning("FatherActor PostStop.... ");
        }


        public static Props Props => Props.Create(() => new FatherActor());
    }

    public class ChildActor : ReceiveActor
    {

        private readonly ILoggingAdapter _logger = Context.GetLogger();
        private int _counter;
        public ChildActor()
        {
            Receive<Next>(_ =>
            {
                _counter++;
                DoSomething();
                Sender.Tell(new Counter(_counter));
            });
        }

        private void DoSomething()
        {
            if (_counter == 5)
                throw new ArithmeticException("5 is too ugly");
            else if (_counter == 9)
                throw new ArgumentException("9 is too ugly");
        }

        public static Props Props => Props.Create(() => new ChildActor());

        protected override void PreStart()
        {
            _logger.Warning("ChildActor PreStart.... ");
        }

        protected override void PostStop()
        {
            _logger.Warning("ChildActor PostStop.... ");
        }
    }


    public class Next { }
    public class Counter
    {
        public int Count { get; private set; }

        public Counter(int counter)
        {
            Count = counter;
        }
    }
}
