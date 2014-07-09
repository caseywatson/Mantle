using System;
using Mantle.Hosting.Hosts;
using Mantle.Hosting.Interfaces;

namespace Mantle.Hosting.Console
{
    public class ConsoleWorkerHost : BaseWorkerHost
    {
        public ConsoleWorkerHost(IWorker worker)
            : base(worker)
        {
        }

        protected override void OnErrorOccurred(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.Error.WriteLine(message);
            System.Console.Error.WriteLine();
        }

        protected override void OnMessageOccurred(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine(message);
            System.Console.WriteLine();
        }
    }
}