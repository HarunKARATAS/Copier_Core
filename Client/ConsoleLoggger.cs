using System;
namespace Copier
{
    class OutputChannel : IOutputChannel
    {
        public void Write(string message)
        {
            Console.WriteLine(message);
        }
    }
}
