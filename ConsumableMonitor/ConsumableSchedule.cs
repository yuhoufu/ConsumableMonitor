using Quartz;
using Quartz.Impl;
using System.Threading.Tasks;

namespace ConsumableMonitor
{
    public class ConsumableSchedule
    {
        readonly IScheduler scheduler = null;

        public ConsumableSchedule()
        {
            // 创建作业调度器
            scheduler = new StdSchedulerFactory().GetScheduler().Result;
            var jobDataMap = new JobDataMap();
            jobDataMap.Add("ConsumableCount1", "1");

            // 创建一个作业
            IJobDetail weeklyJob = JobBuilder.Create<WeeklyJob>()
                .WithIdentity("ConsumableJobTest1", "ConsumableJobGroupTest2")
                .UsingJobData(jobDataMap)
                .Build();

            // 创建一个触发器
            ITrigger weeklyTrigger = TriggerBuilder.Create()
                .WithIdentity("ConsumableTriggerTest1", "ConsumableTriggerGroupTest1")
                .StartNow()
                //.WithSimpleSchedule(x => x
                //    .WithIntervalInSeconds(1)
                //    .WithRepeatCount(10))
                .WithCronSchedule("0/5 * * * * ?")
                .Build();


            var jobExist = scheduler.CheckExists(weeklyJob.Key).Result;
            if (!jobExist)
            {
                 scheduler.ScheduleJob(weeklyJob, weeklyTrigger).Wait();
            }
        }

        public void Start()
        {
            scheduler.Start().Wait();
        }

        public void Stop()
        {
            scheduler.Shutdown().Wait();
        }
    }
}