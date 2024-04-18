namespace MMakerBotPanel.Business.Strategy
{
    using MMakerBotPanel.Business.Strategy.CexGrid;
    using MMakerBotPanel.Business.Strategy.CexGrid.Model;
    using MMakerBotPanel.Business.Strategy.CexMaker.Model;
    using MMakerBotPanel.Business.Strategy.CexSpread;
    using MMakerBotPanel.Business.Strategy.DexGrid;
    using MMakerBotPanel.Business.Strategy.Interfaces;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TradingBotCreator
    {
        private static TradingBotCreator _TradingBotCreator;

        private static readonly object _lock = new object();

        public List<FacIStrategy> WorkingStrategies = new List<FacIStrategy>();


        private TradingBotCreator()
        {

        }

        public static TradingBotCreator GetTradingBotCreator()
        {
            lock (_lock)
            {
                if (_TradingBotCreator == null)
                {
                    _TradingBotCreator = new TradingBotCreator();
                }
                return _TradingBotCreator;
            }
        }

        public bool StartStrategy(object parameters, int workerID)
        {
            int workerType;
            using (ModelContext db = new ModelContext())
            {

                Worker worker = db.Workers.FirstOrDefault(m => m.WorkerID == workerID);
                workerType = (int)worker.Product.workerType;

            }
            FacConcreteCreatorAbstract concreater = null;
            switch (workerType)
            {
                case 1:
                    concreater = CexGridConcreateCreator.GetCexGridConcreateCreator();
                    break;
                case 2:
                    concreater = DexGridConcreateCreator.GetDexGridConcreateCreator();
                    break;
                case 3:
                    concreater = CexMakerConcreateCreator.GetCexSpreadConcreateCreator();
                    break;
                default:

                    break;
            }

            FacIStrategy workingWorker = concreater.Start(parameters , workerID);
            WorkingStrategies.Add(workingWorker);


            return true;
        }

        public bool StopStrategy(int WorkerID)
        {
            FacIStrategy stoppingStrategy = null;
            foreach (FacIStrategy item in WorkingStrategies)
            {
                if (item.WorkerID == WorkerID)
                {
                    stoppingStrategy = item;
                    break;
                }
            }
            if (stoppingStrategy != null)
            {
                int workerType;
                using (ModelContext db = new ModelContext())
                {
                    Worker worker = db.Workers.FirstOrDefault(m => m.WorkerID == WorkerID);
                    workerType = (int)worker.Product.workerType;
                }
                FacConcreteCreatorAbstract concreater = null;
                switch (workerType)
                {
                    case 1:
                        concreater = CexGridConcreateCreator.GetCexGridConcreateCreator();
                        break;
                    case 2:
                        concreater = DexGridConcreateCreator.GetDexGridConcreateCreator();
                        break;
                    case 3:
                        concreater = CexMakerConcreateCreator.GetCexSpreadConcreateCreator();
                        break;
                    default:

                        break;
                }

                concreater.Stop(stoppingStrategy);
                if (WorkingStrategies != null && WorkingStrategies.Contains(stoppingStrategy))
                {
                    _ = WorkingStrategies.Remove(stoppingStrategy);
                }
            }
            return true;
        }



    }
}