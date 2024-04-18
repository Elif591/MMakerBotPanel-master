namespace MMakerBotPanel.WebServices.OKX.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StockDataModel
    {
        public StockDataModel()
        {
            genericResult = new GenericResult();
            data = new List<List<string>>();

            OKXChartDataDetailModels = new List<StockDataDetail>();
        }
        public GenericResult genericResult;
        public string code { get; set; }
        public string msg { get; set; }
        public List<List<string>> data { get; set; }

        public List<StockDataDetail> OKXChartDataDetailModels;
    }
}