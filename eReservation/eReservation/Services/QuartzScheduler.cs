using Quartz;
using Quartz.Impl;
using System;
using System.Threading.Tasks;

public class QuartzScheduler
{
    public static async Task Start()
    {
        IScheduler scheduler = await StdSchedulerFactory.GetDefaultScheduler();
        await scheduler.Start();

        IJobDetail job = JobBuilder.Create<EmailJob>()
            .WithIdentity("emailJob", "group1")
            .Build();

        ITrigger trigger = TriggerBuilder.Create()
            .WithIdentity("emailTrigger", "group1")
            .StartNow()
            .WithSimpleSchedule(x => x
                .WithIntervalInMinutes(1)
                .RepeatForever())
            .Build();

        await scheduler.ScheduleJob(job, trigger);


    }
}
