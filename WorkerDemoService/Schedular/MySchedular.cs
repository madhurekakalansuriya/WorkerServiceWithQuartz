﻿using Microsoft.Extensions.Hosting;
using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WorkerDemoService.Models;

namespace WorkerDemoService.Schedular
{// start and stop task
    class MySchedular : IHostedService
    {
        public IScheduler Scheduler { get; set; }
        private readonly IJobFactory jobFactory;
        //private readonly List<JobMetadata> jobMetadatas;
        private readonly JobMetadata jobMetadata;
        private readonly ISchedulerFactory schedulerFactory;

        public MySchedular(ISchedulerFactory schedulerFactory,JobMetadata jobMetadata,IJobFactory jobFactory)
        {
            this.jobFactory = jobFactory;
            this.schedulerFactory = schedulerFactory;
            this.jobMetadata = jobMetadata;
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            //Creating Schdeular
            Scheduler = await schedulerFactory.GetScheduler();
            Scheduler.JobFactory = jobFactory;

            ////Suporrt for Multiple Jobs
            //jobMetadatas?.ForEach(jobMetadata =>
            //{
            //    //Create Job
            //    IJobDetail jobDetail = CreateJob(jobMetadata);
            //    //Create trigger
            //    ITrigger trigger = CreateTrigger(jobMetadata);
            //    //Schedule Job
            //    Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken).GetAwaiter();
            //    //Start The Schedular
            //});


            //Create Job
            IJobDetail jobDetail = CreateJob(jobMetadata);
            //Create trigger
            ITrigger trigger = CreateTrigger(jobMetadata);
            //Schedule Job
            Scheduler.ScheduleJob(jobDetail, trigger, cancellationToken).GetAwaiter();
            //Start The Schedular
            await Scheduler.Start(cancellationToken);
        }

        private ITrigger CreateTrigger(JobMetadata jobMetadata)
        {
            return TriggerBuilder.Create()
                .WithIdentity(jobMetadata.JobId.ToString())
                .WithCronSchedule(jobMetadata.CronExpression)
                .WithDescription(jobMetadata.JobName)
                .Build();
        }

        private IJobDetail CreateJob(JobMetadata jobMetadata)
        {
            return JobBuilder.Create(jobMetadata.JobType)
                .WithIdentity(jobMetadata.JobId.ToString())
                .WithDescription(jobMetadata.JobName)
                .Build();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Scheduler.Shutdown();
        }
    }
}
