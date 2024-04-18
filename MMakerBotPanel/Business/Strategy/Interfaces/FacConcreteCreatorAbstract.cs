namespace MMakerBotPanel.Business.Strategy.Interfaces
{
    using MMakerBotPanel.Business.Services;
    using MMakerBotPanel.Business.Strategy.CexGrid;
    using MMakerBotPanel.Business.Strategy.CexGrid.Model;
    using MMakerBotPanel.Business.Strategy.CexSpread;
    using MMakerBotPanel.Business.Strategy.DexGrid;
    using MMakerBotPanel.Database.Context;
    using MMakerBotPanel.Database.Model;
    using System.Linq;

    public abstract class FacConcreteCreatorAbstract
    {
        public FacConcreteCreatorAbstract()
        {

        }

        public FacIStrategy Start(object parameters , int workerID)
        {
            int workerType;
            using (ModelContext db = new ModelContext())
            {
                Worker worker = db.Workers.FirstOrDefault(m => m.WorkerID == workerID);
                workerType = (int)worker.Product.workerType;
            }
            FacStrategyObserver FacStrategyObserver = null;
            switch (workerType)
            {
                case 1:
                    FacStrategyObserver = new CexGridStrategy(parameters);
                    break;
                case 2:
                    FacStrategyObserver = new DexGridStrategy(parameters); 
                    break;
                case 3:
                    FacStrategyObserver = new CexMakerStrategy(parameters);
                    break;
                default:

                    break;
            }

            if (BotStartingMethods(parameters))
            {
                FacStrategyObserver.Subscribe(TaskSchedulerService.GetTaskSchedulerService());
                return ((FacIStrategy)FacStrategyObserver);
            }
            else
            {
                return null;
            }
        }

        public bool Stop(FacIStrategy facStrategyObserver)
        {
            try
            {
                ((FacStrategyObserver)facStrategyObserver).Unsubscribe();
                
                BotStoppingMethods(facStrategyObserver);
                return true;
            }
            catch (System.Exception)
            {
                return false;
            }

        }

        public abstract bool BotStartingMethods(object parameters);

        public abstract bool BotStoppingMethods(FacIStrategy facStrategyObserver);
    }
}