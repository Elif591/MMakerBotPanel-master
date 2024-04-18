namespace MMakerBotPanel.Business.Strategy.DexGrid
{
    using MMakerBotPanel.Business.Services.Models;
    using MMakerBotPanel.Business.Strategy.CexGrid.Model;
    using MMakerBotPanel.Business.Strategy.Interfaces;
    using MMakerBotPanel.Models;

    public class DexGridStrategy : FacStrategyObserver
    {
        public DexGridStrategy(object parameters)
        {
            CexGridParameterModel dexGridParameter = (CexGridParameterModel)parameters;
            workerID = dexGridParameter.workerID;
            type = WORKERTYPE.DexGridWorker;
        }

        public override void OnNext(ObserverMessage value)
        {
            throw new System.NotImplementedException();
        }

        public override void Tick()
        {
            throw new System.NotImplementedException();
        }
    }
}