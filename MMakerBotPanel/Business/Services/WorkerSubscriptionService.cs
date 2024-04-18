namespace MMakerBotPanel.Business.Services
{
    using MMakerBotPanel.Business.Services.Models;
    using MMakerBotPanel.Business.Strategy;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class WorkerSubscriptionService : ServicesInterfaces
    {
        private static WorkerSubscriptionService _WorkerSubscriptionService;

        private static readonly object _locker = new object();

        private WorkerSubscriptionService() { }

        public static WorkerSubscriptionService GetWorkerSubscriptionService()
        {
            lock (_locker)
            {
                if (_WorkerSubscriptionService == null)
                {
                    _WorkerSubscriptionService = new WorkerSubscriptionService();
                }
                return _WorkerSubscriptionService;
            }
        }

        public override void Tick()
        {
            using (ModelContext db = new ModelContext())
            {
                List<Worker> workers = db.Workers.Where(x => x.WorkerState != WORKERSTATE.Expired).ToList();
                foreach (Worker worker in workers)
                {
                    worker.DaysRemaining = Convert.ToInt32((worker.EndingDate - DateTime.Now).TotalDays);
                    if (worker.DaysRemaining < 0)
                    {
                        worker.WorkerState = WORKERSTATE.Expired;
                        TradingBotCreator.GetTradingBotCreator().StopStrategy(worker.WorkerID);
                    }
                }
                _ = db.SaveChanges();
            }


        }

        public override void OnNext(ObserverMessage value)
        {
            DateTime dateTime = DateTime.Now;
            CacheHelper.Add<Int64>(CACHEKEYENUMTYPE.WorkerSubscriptionService_LastActivity.ToString(), Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds), DateTimeOffset.Now.AddSeconds(10));
            if (dateTime.Hour == 0 && dateTime.Minute == 0 && dateTime.Second == 0)
            {
                Tick();
            }
        }

    }
}