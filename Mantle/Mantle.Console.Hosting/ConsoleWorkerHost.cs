using System;
using Mantle.Hosting;

namespace Mantle.Console.Hosting
{
    public class ConsoleWorkerHost : BaseWorkerHost
    {
        public ConsoleWorkerHost(IDependencyResolver dependencyResolver)
            : base(dependencyResolver)
        {
        }

        protected override void OnErrorOccurred(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.Red;
            System.Console.WriteLine(message);
            System.Console.WriteLine();
        }

        protected override void OnMessageOccurred(string message)
        {
            System.Console.ForegroundColor = ConsoleColor.White;
            System.Console.WriteLine(message);
            System.Console.WriteLine();
        }
    }
}