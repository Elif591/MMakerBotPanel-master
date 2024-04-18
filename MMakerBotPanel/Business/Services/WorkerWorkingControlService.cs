namespace MMakerBotPanel.Business.Services
{
    using MMakerBotPanel.Business.Services.Models;
    using MMakerBotPanel.Business.Strategy;
    using MMakerBotPanel.Business.Strategy.Interfaces;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI.WebControls;

    public class WorkerWorkingControlService : ServicesInterfaces
    {
        private static WorkerWorkingControlService _WorkerWorkingControlService;

        private static readonly object _locker = new object();

        private int sayac = 0;

       // private readonly List<TradingBotInterface> TradingBots = new List<TradingBotInterface>();

        private WorkerWorkingControlService() { }

        public static WorkerWorkingControlService GetWorkerWorkingControlService()
        {
            lock (_locker)
            {
                if (_WorkerWorkingControlService == null)
                {
                    _WorkerWorkingControlService = new WorkerWorkingControlService();
                }
                return _WorkerWorkingControlService;
            }
        }

        public override void OnNext(ObserverMessage value)
        {
            sayac++;
            CacheHelper.Add<Int64>(CACHEKEYENUMTYPE.WorkerWorkingControlService_LastActivity.ToString(), Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds), DateTimeOffset.Now.AddSeconds(10));
            if (sayac == 10)
            {
                sayac = 0;
                Tick();
            }
        }

        public override void Tick()
        {
            using (ModelContext db = new ModelContext())
            {
                List<Worker> WorkingWorkers = db.Workers.ToList();
                List<Worker> removeWorker = new List<Worker>();
                List<FacIStrategy> workerStrategies = TradingBotCreator.GetTradingBotCreator().WorkingStrategies;
                foreach (Worker worker in WorkingWorkers)
                {
                    bool eslesti = false;
                    foreach (FacIStrategy TradingBot in workerStrategies)
                    {
                        if (worker.WorkerID == TradingBot.WorkerID)
                        {
                            worker.WorkerState = WORKERSTATE.Working;
                            eslesti = true;
                            break;
                        }
                    }
                    if (!eslesti)
                    {
                        worker.WorkerState = WORKERSTATE.Paused;
                        removeWorker.Add(worker);
                    }
                }
                foreach (Worker worker in removeWorker)
                {
                    _ = WorkingWorkers.Remove(worker);
                }


                //List<TradingBotInterface> removeTradingBot = new List<TradingBotInterface>();
                //foreach (TradingBotInterface TradingBot in removeTradingBot)
                //{
                //    bool eslesti = false;
                //    foreach (Worker worker in WorkingWorkers)
                //    {
                //        if (TradingBot._Worker.WorkerID == worker.WorkerID)
                //        {
                //            eslesti = true;
                //            break;
                //        }
                //    }
                //    if (eslesti)
                //    {
                //        bool state = CacheHelper.Get<bool>("Working_" + TradingBot._Worker.WorkerID.ToString());

                //        if (!state)
                //        {
                //            if (TradingBot._ExchangeType == EXCHANGETYPE.CEX)
                //            {
                //                //((CexTradingBot)TradingBot).Unsubscribe();
                //            }
                //            else
                //            {

                //            }
                //            removeTradingBot.Add(TradingBot);
                //            Worker _worker = WorkingWorkers.First(x => x.WorkerID == TradingBot._Worker.WorkerID);
                //            _worker.WorkerState = WORKERSTATE.Paused;
                //        }
                //    }
                //    else
                //    {
                //        if (TradingBot._ExchangeType == EXCHANGETYPE.CEX)
                //        {
                //            //((CexTradingBot)TradingBot).Unsubscribe();
                //        }
                //        else
                //        {

                //        }
                //        removeTradingBot.Add(TradingBot);
                //    }
                //}
                //foreach (TradingBotInterface TradingBot in removeTradingBot)
                //{
                //    _ = TradingBots.Remove(TradingBot);
                //}


                _ = db.SaveChanges();
            }
        }
    }
}