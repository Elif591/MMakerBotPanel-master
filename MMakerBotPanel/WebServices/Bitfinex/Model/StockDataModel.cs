namespace MMakerBotPanel.WebServices.Bitfinex.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StockDataModel
    {
        public StockDataModel()
        {
            genericResult = new GenericResult();
            stockDataDetailModels = new List<StockDataDetail>();
            data = new Data();
        }
        public GenericResult genericResult;
        public List<StockDataDetail> stockDataDetailModels;
        public Data data;

    }
}