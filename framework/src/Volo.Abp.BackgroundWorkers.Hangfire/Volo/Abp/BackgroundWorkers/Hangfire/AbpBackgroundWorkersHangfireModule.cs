using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Hangfire;
using Volo.Abp.Modularity;

namespace Volo.Abp.BackgroundWorkers.Hangfire
{
    [DependsOn(
        typeof(AbpBackgroundWorkersModule),
        typeof(AbpHangfireModule)
    )]
    public class AbpBackgroundWorkersHangfireModule: AbpModule
    {
        public override void PreConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddConventionalRegistrar(new AbpHangfireConventionalRegistrar());
        }

        public override void OnApplicationInitialization(ApplicationInitializationContext context)
        {
            var backgroundWorkerManager = context.ServiceProvider.GetService<IBackgroundWorkerManager>();
            var works = context.ServiceProvider.GetServices<IHangfireBackgroundWorker>();

            foreach (var work in works)
            {
                backgroundWorkerManager.Add(work);
            }
        }
    }
}