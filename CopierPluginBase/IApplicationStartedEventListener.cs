using System;
namespace CopierPluginBase
{
    public interface IApplicationStartedEventListener
    {
        void OnApplicationStarted(string filePath);
    }
}
