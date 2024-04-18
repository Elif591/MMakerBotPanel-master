namespace MMakerBotPanel.WebServices.Kucoin.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StockDataModel
    {
        public StockDataModel()
        {
            genericResult = new GenericResult();
            data = new List<List<string>>();
            KucoinDataDetailModels = new List<StockDataDetail>();
        }
        public string code { get; set; }
        public List<List<string>> data;
        public GenericResult genericResult;
        public List<StockDataDetail> KucoinDataDetailModels;

    }

}