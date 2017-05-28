using System;
using System.Diagnostics;
using System.Linq;
using System.Web.Hosting;
using System.Threading;

namespace DEVES.IntegrationAPI.WebApi.TechnicalService.TransactionLogger
{
    public static class LogJobHandle
    {
        
        private static readonly Timer timer = new Timer(OnTimerElapsed);
        private static readonly LogJob logJob = new LogJob();

        public static void Start()
        {
            Debug.WriteLine("Log Job Start");
            timer.Change(TimeSpan.Zero, TimeSpan.FromMinutes(1));
        }

        private static void OnTimerElapsed(object sender)
        {
            logJob.DoWork(() =>
            {
                /* What is it that you do around here */
                Debug.WriteLine("logJob:DoWork ");
                Debug.WriteLine("log:Count " + InMemoryLogData.Instance.LogData.Count);
                if (InMemoryLogData.Instance.LogData.Any())
                {
                    for (var i=0; i<= InMemoryLogData.Instance.LogData.Count; i++)
                    {
                        ApiLogEntry log = InMemoryLogData.Instance.LogData.FirstOrDefault();
                        if (log != null )
                        {
                            if (!log.IsPersisted)
                            {
                                ApiLogDataGateWay.Create(log);
                               
                            }
                           
                            InMemoryLogData.Instance.LogData.Remove(log);
                        }
                       

                    }
                }
                
                
            });
        }
    }
    public class LogJob : System.Web.Hosting.IRegisteredObject
    {
        private readonly object _lock = new object();
        private bool _shuttingDown;

        public LogJob()
        {
            HostingEnvironment.RegisterObject(this);
        }

        public void Stop(bool immediate)
        {
            lock (_lock)
            {
                _shuttingDown = true;
            }
            HostingEnvironment.UnregisterObject(this);
        }

        public void DoWork(Action work)
        {
            lock (_lock)
            {
                if (_shuttingDown)
                {
                    return;
                }
                work();
            }
        }
    }
   
}