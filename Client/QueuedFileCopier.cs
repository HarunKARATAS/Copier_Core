using System;
namespace Copier
{
    public class QueuedFileCopier : IFileCopier
    {
        private CommandOptions options;
        private ILogger logger;

        public QueuedFileCopier(CommandOptions options, ILogger logger)
        {
            this.options = options;
            this.logger = logger;
        }

        public void CopyFile(CommandOptions options, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
