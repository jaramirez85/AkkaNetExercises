using System.Threading;

namespace AkkaExercises.Sample4.ExternalSystems
{
    static class FakePaymentGateway
    {
        public static void Pay(int accountNumber, decimal amount)
        {
            // Simulate communicating with external payment gateway
            Thread.Sleep(200);            
        }
    }
}
