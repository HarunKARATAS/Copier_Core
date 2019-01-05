using System;
namespace Copier
{
    class ConsoleLoggger : ILogger
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
