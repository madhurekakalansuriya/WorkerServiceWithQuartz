﻿using Quartz;
using Quartz.Spi;
using System;
using System.Collections.Generic;
using System.Text;

namespace WorkerDemoService.JobFactory
{
    class MyJobFactory : IJobFactory
    {
        private readonly IServiceProvider service;
        public MyJobFactory(IServiceProvider serviceProvider)
        {
            service = serviceProvider;
        }

        //create a new job
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            //details create a s bundle
            var jobDetail = bundle.JobDetail;
            return (IJob)service.GetService(jobDetail.JobType);
        }

        public void ReturnJob(IJob job)
        {
            
        }
    }
}
