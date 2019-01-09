using System;
using CopierPluginBase;

namespace SamplePlugin
{
    public class SamplePostCopy:IPostCopyEventListener
    {
        public void OnPostCopy(string filePath)
        {
            Console.WriteLine("Sample Plugin OnPost Copy Event Listener executed.");
        }
    }
}
