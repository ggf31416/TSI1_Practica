using Quartz;
using Quartz.Impl;
using Shared.Entities;
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
            
            BLTablero.getInstancia().ejecutarBatallasEnCurso();
        }
    }

    class IniciarBatallaJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            
        }

        
    }

    public class Planificador
    {

        private static Planificador instancia;
        private IScheduler scheduler;

        public static  Planificador getInstancia()
        {
            if (instancia == null) instancia = new Planificador();
            return instancia;
        }

        private Planificador()
        {

        }


        private ITrigger agregarTriggerSimple(int segundosDelay)
        {
            ITrigger t = TriggerBuilder.Create().StartNow().WithSimpleSchedule(x =>
                    x.WithInterval(TimeSpan.FromMilliseconds(1000 * segundosDelay)))
                    .Build();
            return t;

        }

        private void unaVez(IJobDetail job, int segundosDelay)
        {
            ITrigger t = agregarTriggerSimple(segundosDelay);
            scheduler.ScheduleJob(job, t);
        }


        public void IniciarAtaque(string tenant,InfoAtaque ataque,int segundosDelay)
        {
            var job = JobBuilder.Create<IniciarBatallaJob>().WithIdentity("job1", "group1").Build();


        }

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
                     x.WithInterval(TimeSpan.FromMilliseconds(1000)).RepeatForever())
                     .Build();
                scheduler.ScheduleJob(turnosJob, trigger);
            }
            catch (SchedulerException se)
            {

                Console.WriteLine("Quartz Ex:" + se.ToString());
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

                Console.WriteLine("Quartz Ex:" + se.ToString());
            }
        }
    }
}
