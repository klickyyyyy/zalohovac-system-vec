using System.Text.Json;
using Quartz;
using Quartz.Impl;

namespace Zalohovac
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            // nacitani z api
            var jobsFromServer = ApiLoader.LoadFromServer();

            if (jobsFromServer.Count == 0)
            {
                Console.WriteLine("Zadne ulohy nenalezeny nebo se nepodarilo pripojit k serveru.");
                Console.ReadLine();
                return;
            }

            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = await factory.GetScheduler();
            await scheduler.Start();

            foreach (var config in jobsFromServer)
            {
                IJobDetail job = JobBuilder.Create<BackupJobExecutor>()
                    .WithIdentity("Zaloha_" + Guid.NewGuid().ToString())
                    .UsingJobData("jobConfig", JsonSerializer.Serialize(config))
                    .Build();

                ITrigger trigger = TriggerBuilder.Create()
                    .WithCronSchedule(config.Timing + " ?")
                    .Build();

                await scheduler.ScheduleJob(job, trigger);
            }

            Console.WriteLine("Plánovač běží. Program ukončíte stisknutím klávesy Enter...");
            Console.ReadLine();
        }
    }
}
