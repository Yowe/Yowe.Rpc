using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RpcFrameWork
{
    internal class DefaultTaskCreationOptionProvider
    {
        private readonly int _availableWorkerThreshold;

        public DefaultTaskCreationOptionProvider(int availableWorkerThreshold = 4)
        {
            _availableWorkerThreshold = availableWorkerThreshold;
        }

        public TaskCreationOptions GetOptions()
        {
            int worker;
            int ioCompletion;

            ThreadPool.GetAvailableThreads(out worker, out ioCompletion);

#if DEBUG
            Global.DefaultWatcher.DebugFormat("Available workers in ThreadPool: {0}. The number of available asynchronous I/O threads: {1} ", worker, ioCompletion);
#endif
            if (worker > _availableWorkerThreshold)
            {
                return TaskCreationOptions.PreferFairness;
            }
            return TaskCreationOptions.LongRunning;
        }
    }
}
