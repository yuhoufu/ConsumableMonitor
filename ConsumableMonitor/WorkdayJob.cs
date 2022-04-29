using Quartz;
using System;
using System.Threading.Tasks;

namespace ConsumableMonitor
{

    [DisallowConcurrentExecution]
    [PersistJobDataAfterExecution]
    public class WorkdayJob : IJob
    {
        /// <summary>
        /// 作业调度定时执行的方法,每周执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            var WorkdayCount = Convert.ToInt32(context.JobDetail.JobDataMap["WorkdayCount"]);
            context.JobDetail.JobDataMap["WorkdayCount"] = (WorkdayCount + 1).ToString();

            await Console.Out.WriteLineAsync($"Workday: execute {WorkdayCount} times");

            await Task.CompletedTask;
        }
        

    }
}