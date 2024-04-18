namespace MMakerBotPanel.WebServices.Binance.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StockDataModel
    {
        public StockDataModel()
        {
            genericResult = new GenericResult();
            stockDataDetailModels = new List<StockDataDetail>();
        }

        public List<StockDataDetail> stockDataDetailModels;
        public GenericResult genericResult;
    }
}