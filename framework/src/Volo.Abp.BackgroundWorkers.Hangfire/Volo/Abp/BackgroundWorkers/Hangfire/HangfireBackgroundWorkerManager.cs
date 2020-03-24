using System;
using System.Threading;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Volo.Abp.DependencyInjection;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    [Dependency(ReplaceServices = true)]
    public class HangfireBackgroundWorkerManager : IBackgroundWorkerManager, ISingletonDependency
    {
        private ILogger<HangfireBackgroundWorkerManager> Logger { get; set; }

        public HangfireBackgroundWorkerManager()
        {
            Logger = NullLogger<HangfireBackgroundWorkerManager>.Instance;
        }

        public Task StartAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("Started background worker: " + ToString());
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            Logger.LogDebug("Stopped background worker: " + ToString());
            return Task.CompletedTask;
        }

        public void Add(IBackgroundWorker worker)
        {
            if (worker is IHangfireBackgroundWorker hangfireWork)
            {
                Check.NotNull(hangfireWork.CronExpression, nameof(hangfireWork.CronExpression));

                RecurringJob.AddOrUpdate(() => hangfireWork.DoWorkAsync(), hangfireWork.CronExpression, TimeZoneInfo.Local);
            }
        }
    }
}