using System.Collections.Generic;
using System.IO;
using System.Linq;
using Akka.Actor;
using Akka.Routing;
using AkkaExercises.Sample4.Messages;

namespace AkkaExercises.Sample4.Actors
{
    // Assumes we only get one file per invocation of the console application
    internal class JobCoordinatorActor : ReceiveActor     
    {
        private readonly IActorRef _paymentWorker;
        private int _numberOfRemainingPayments;

        public JobCoordinatorActor()
        {
            //With RoundRobinGroup
            _paymentWorker = Context.ActorOf(
                Props.Empty.WithRouter(new RoundRobinGroup(
                    "/user/PaymentWorker1", 
				    "/user/PaymentWorker2", 
				    "/user/PaymentWorker3"
                    )));

            Receive<ProcessFileMessage>(
                message =>
                {
                    StartNewJob(message.FileName);
                });


            Receive<PaymentSentMessage>(
                message =>
                {
                    _numberOfRemainingPayments--;

                    var jobIsComplete = _numberOfRemainingPayments == 0;

                    if (jobIsComplete)
                    {
                        Context.System.Terminate();
                    }
                });
        }


        private void StartNewJob(string fileName)
        {
            List<SendPaymentMessage> requests = ParseCsvFile(fileName);

            _numberOfRemainingPayments = requests.Count();

            foreach (var sendPaymentMessage in requests)
                _paymentWorker.Tell(sendPaymentMessage);
        }



        // This could be delegated to a lower level actor to act like an error handler
        private List<SendPaymentMessage> ParseCsvFile(string fileName)
        {
            var messagesToSend = new List<SendPaymentMessage>();

            var fileLines = File.ReadAllLines(fileName);

            foreach (var line in fileLines)
            {
                var values = line.Split(',');

                var message = new SendPaymentMessage(
                                    values[0],
                                    values[1],
                                    int.Parse(values[3]),
                                    decimal.Parse(values[2]));

                messagesToSend.Add(message);
            }

            return messagesToSend;
        }
     
    }
}