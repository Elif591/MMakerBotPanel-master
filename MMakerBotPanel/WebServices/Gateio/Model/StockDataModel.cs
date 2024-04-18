namespace MMakerBotPanel.WebServices.Gateio.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StockDataModel
    {
        public StockDataModel()
        {
            genericResult = new GenericResult();
            GateioData = new List<List<string>>();
            GateioDataDetailModels = new List<StockDataDetail>();
        }

        public GenericResult genericResult;
        public List<List<string>> GateioData;
        public List<StockDataDetail> GateioDataDetailModels;

    }
}
