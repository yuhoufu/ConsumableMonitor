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
            var weeklyJobDataMap = new JobDataMap();
            weeklyJobDataMap.Add("WeeklyCount", "1");
            var workdayJobDataMap = new JobDataMap();
            workdayJobDataMap.Add("WorkdayCount", "1");

            // 创建一个作业
            IJobDetail weeklyJob = JobBuilder.Create<WeeklyJob>()
                .WithIdentity("WeeklyJob", "WeeklyJobGroup")
                .UsingJobData(weeklyJobDataMap)
                .Build();

            // 创建一个触发器
            ITrigger weeklyTrigger = TriggerBuilder.Create()
                .WithIdentity("WeeklyTrigger", "WeeklyTriggerGroup")
                .StartNow()
                //.WithSimpleSchedule(x => x
                //    .WithIntervalInSeconds(1)
                //    .WithRepeatCount(10))
                .WithCronSchedule("0/5 * * * * ?")
                .Build();


            // 创建一个工作日常作业
            IJobDetail workdayJob = JobBuilder.Create<WorkdayJob>()
                .WithIdentity("WorkdayJob", "WorkdayJobGroup")
                .UsingJobData(workdayJobDataMap)
                .Build();

            // 创建一个工作日常触发器
            ITrigger workdayTrigger = TriggerBuilder.Create()
                .WithIdentity("WorkdayTrigger", "WorkdayTriggerGroup")
                .StartNow()
                //.WithSimpleSchedule(x => x
                //    .WithIntervalInSeconds(1)
                //    .WithRepeatCount(10))
                .WithCronSchedule("* * * * * ?")
                .Build();




            var weeklyJobExist = scheduler.CheckExists(weeklyJob.Key).Result;
            if (!weeklyJobExist)
            {
                scheduler.ScheduleJob(weeklyJob, weeklyTrigger).Wait();
            }

            //var workdayJobExist = scheduler.CheckExists(workdayJob.Key).Result;
            //if (!workdayJobExist)
            //{
            //    scheduler.ScheduleJob(workdayJob, workdayTrigger).Wait();
            //}
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