using System;
using CopierPluginBase;

namespace SamplePlugin
{
    public class SamplePreCopyEventListener : IPreCopyEventListener
    {
        public void OnPreCopy(string filePath)
        {
            Console.WriteLine("Sample Plugin PreCopy Event Listener executed.");
        }
    }
}
