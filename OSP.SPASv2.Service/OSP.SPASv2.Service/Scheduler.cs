using Quartz;

namespace OSP.SPASv2.Service
{
    public static class  Scheduler
    {
        public static void AddInfrastructure(this IServiceCollection services)
        {
            services.AddQuartz(options =>
            {
                options.UseMicrosoftDependencyInjectionJobFactory();

                var jobKey = JobKey.Create(nameof(LoggingBackgroundJob));
                options
                    .AddJob<LoggingBackgroundJob>(jobKey)
                    .AddTrigger(trigger =>
                    trigger
                        .ForJob(jobKey)
                        .WithSimpleSchedule(schedule =>
                        schedule.WithIntervalInSeconds(10000).RepeatForever()));
            }
            );
            services.AddQuartzHostedService();
        }
    }
}
