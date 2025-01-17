using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;
using WorkerDemoService.JobFactory;
using WorkerDemoService.Jobs;
using WorkerDemoService.Models;
using WorkerDemoService.Schedular;

namespace WorkerDemoService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddSingleton<IJobFactory, MyJobFactory>();
                    //already implemented in quatz for shedule factory
                    services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();                    

                    #region Adding JobType
                    services.AddSingleton<NotificationJob>();
                    //services.AddSingleton<LoggerJob>();
                    #endregion

                    #region Adding Jobs 
                    // adding single meta data
                    var jobMetadata = new JobMetadata(Guid.NewGuid(), typeof(NotificationJob), "Notify Job", "0/20 * * * * ?");
                    services.AddSingleton(jobMetadata);

                    // adding multiple meta data
                    //List<JobMetadata> jobMetadatas = new List<JobMetadata>();
                    // jobMetadatas.Add(new JobMetadata(Guid.NewGuid(), typeof(NotificationJob), "Notify Job", "0/20 * * * * ?"));
                    //jobMetadatas.Add(new JobMetadata(Guid.NewGuid(), typeof(LoggerJob), "Log Job", "0/20 * * * * ?"));

                    // services.AddSingleton(jobMetadatas);
                    #endregion

                    services.AddHostedService<MySchedular>();
                });
    }
}
