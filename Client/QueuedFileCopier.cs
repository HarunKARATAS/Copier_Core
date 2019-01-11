using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Copier
{
    public class QueuedFileCopier : IFileCopier,IPreCopyEventBroadcaster,IPostCopyEventBroadcaster
    {

        public event Action<string> PreCopyEvent = delegate { };
        public event Action<string> PostCopyEvent = delegate { };

        private IFileCopier _fileCopier;
        private ILogger _logger;
        private CommandOptions _options;

        private HashSet<string> _fileNameQueue = new HashSet<string>();
        private Task _copyTask;

        public QueuedFileCopier(IFileCopier fileCopier,ILogger logger, CommandOptions options)
        {
           
            _fileCopier = fileCopier;
            _logger = logger;
            _options = options;
            if (_options.Debug)
            {
                _logger.LogDebug($"Delay option has been specified. QueuedFileCopier is chosen as the copier strategy.");
            }
        }


        public void CopyFile(string fileName)
        {
            if (_copyTask == null)
            {
                _copyTask = Task.Run(async () =>
                {

                    await Task.Delay(TimeSpan.FromMilliseconds(_options.Delay));
                    if (_options.Verbose || _options.Debug)
                    {
                        _logger.LogInfo($"{_options.Delay} miliseconds have passed. The Copy Operastion has started...");
                    }
                    PreCopyEvent("");
                    foreach (var item in _fileNameQueue)
                    {

                        _fileCopier.CopyFile(item);
                    }
                    PostCopyEvent("");
                    _copyTask = null;
                    if (_options.Verbose || _options.Debug)
                    {
                        _logger.LogInfo($"The Copy Operastion has finished...");
                        _logger.LogInfo($"Queue hs been emptied...");
                    }
                });
            }
            if (!_fileNameQueue.Contains(fileName))
            {
                if (_options.Verbose || _options.Debug) {
                    _logger.LogInfo($"{fileName} has been added to queue and will be copied over in {_options.Delay} miliseconds");
                }
                _fileNameQueue.Add(fileName);
            }else if (_options.Debug)
            {
                _logger.LogInfo($"{fileName} exist in the file queue, thereby skipped");
            }
        }

       
    }
}
