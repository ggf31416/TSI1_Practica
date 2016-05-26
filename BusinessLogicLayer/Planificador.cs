using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{


    class TurnoJub : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            // ejecutarTurno
            Console.WriteLine("Q "  + DateTime.Now.Millisecond + " ms");
        }
    }

    public class Planificador
    {

        private static Planificador instancia;
        public static  Planificador getInstancia()
        {
            if (instancia == null) instancia = new Planificador();
            return instancia;
        }


        IScheduler scheduler;

        public void iniciar()
        {
            try
            {
                // Grab the Scheduler instance from the Factory 
                scheduler = StdSchedulerFactory.GetDefaultScheduler();

                // and start it off
                scheduler.Start();

                IJobDetail turnosJob = JobBuilder.Create<TurnoJub>().WithIdentity("job1", "group1").Build();
                ITrigger trigger = TriggerBuilder.Create().WithIdentity("trigger1", "group1").StartNow().WithSimpleSchedule(x =>
                     x.WithInterval(TimeSpan.FromMilliseconds(100)).RepeatForever())
                     .Build();
                scheduler.ScheduleJob(turnosJob, trigger);
            }
            catch (SchedulerException se)
            {

                Console.WriteLine("Quartz Ex:" + se);
            }
        }

        public void finalizar()
        {
            try
            {
                scheduler.Shutdown();
            }
            catch (SchedulerException se)
            {

                Console.WriteLine("Quartz Ex:" + se);
            }
        }
    }
}
