using Akka.Actor;
using Akka.Configuration;
using RemoteDeploy.Shared.Actor;
using RemoteDeploy.Shared.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RemoteDeploy.Deployer
{
    public class Program
    {
        class SayHello { }

        class HelloActor : ReceiveActor
        {
            private IActorRef _remoteActor;
            private int _helloCounter;
            private ICancelable _helloTask;

            public HelloActor(IActorRef remoteActor)
            {
                _remoteActor = remoteActor;
                Context.Watch(_remoteActor); // <-- this is the only call needed for registration

                Receive<Hello>(hello =>
                {
                    Console.WriteLine("Received {1} from {0}", Sender, hello.Message);
                });

                Receive<SayHello>(sayHello =>
                {
                    _remoteActor.Tell(new Hello("hello" + _helloCounter++));
                    //Context.ActorSelection("/user/remoteecho").Tell(new Hello("hi from selection 1!"));
                });

                Receive<Terminated>(terminated =>
                {
                    Console.WriteLine(terminated.ActorRef);
                    Console.WriteLine("Was address terminated? {0}", terminated.AddressTerminated);
                    _helloTask.Cancel();
                });
            }

            protected override void PreStart()
            {
                _helloTask = Context.System.Scheduler.ScheduleTellRepeatedlyCancelable(
                    TimeSpan.FromSeconds(1),
                    TimeSpan.FromSeconds(10), 
                    Context.Self, 
                    new SayHello(), 
                    ActorRefs.NoSender);
            }

            protected override void PostStop()
            {
                _helloTask.Cancel();
            }
        }


        static void Main(string[] args)
        {
            Console.Title = "Deployer";
            using (var system = ActorSystem.Create("Deployer", ConfigurationFactory.ParseString(@"
                akka {  
                    actor{
                        provider = ""Akka.Remote.RemoteActorRefProvider, Akka.Remote""
                        deployment {
                            /remoteecho {
                                remote = ""akka.tcp://DeployTarget@localhost:8090""
                            }
                        }
                    }
                    remote {
                        helios.tcp {
		                    port = 0
		                    hostname = localhost
                        }
                    }
                }")))
            {
                var remoteAddress = Address.Parse("akka.tcp://DeployTarget@localhost:8090");
                var remoteEcho1 = system.ActorOf(Props.Create(() => new EchoActor()), "remoteecho"); //deploy remotely via config
                var remoteEcho2 =
                    system.ActorOf(
                        Props.Create(() => new EchoActor())
                            .WithDeploy(Deploy.None.WithScope(new RemoteScope(remoteAddress))), "coderemoteecho"); //deploy remotely via code

                system.ActorOf(Props.Create(() => new HelloActor(remoteEcho1)));
                system.ActorOf(Props.Create(() => new HelloActor(remoteEcho2)));

                Console.ReadKey();

                //When you send a message from outside an actor, Akka fills in the dead letters mailbox as the sender
                system.ActorSelection("/user/remoteecho").Tell(new Hello("hi from selection 1!"));
                system.ActorSelection("/user/coderemoteecho").Tell(new Hello("hi from selection 2!"));

                Console.ReadKey();
            }
        }
    }
}
