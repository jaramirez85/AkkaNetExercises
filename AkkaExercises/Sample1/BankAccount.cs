using Akka.Actor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AkkaExercises.Sample1
{
    class BanckAccount
    {
        //static object lockObject = new object();

        static void Main(string[] args)
        {

            var account = new BankAccount();
            var tasks = new List<Task>();

            for (int i = 0; i < 20; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    for (int j = 0; j < 1000; j++)
                        account.Balance = account.Balance + 1;
                }));
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(string.Concat("Expected value 20000", ", actual value ", account.Balance));

            Console.ReadKey();
        }


    }

    class BankAccount
    {
        public int Balance { get; set; }
    }
}
