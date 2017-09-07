using System;
using Akka.Actor;
using AkkaExercises.Sample4.Messages;
using AkkaExercises.Sample4.ExternalSystems;

namespace AkkaExercises.Sample4.Actors
{ 
    internal class PaymentWorkerActor : ReceiveActor
    {

        public PaymentWorkerActor()
        {
            Receive<SendPaymentMessage>(message => SendPayment(message));
        }

        private void SendPayment(SendPaymentMessage message)
        {
            Console.WriteLine("Sending payment for {0} {1}", message.FirstName, message.LastName);
            FakePaymentGateway.Pay(message.AccountNumber, message.Amount);
            Sender.Tell(new PaymentSentMessage(message.AccountNumber));
        }
    }
}