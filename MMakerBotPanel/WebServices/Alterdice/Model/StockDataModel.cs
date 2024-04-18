namespace MMakerBotPanel.WebServices.Alterdice.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StockDataModel
    {
        public StockDataModel()
        {
            stockDataDetailModels = new List<StockDataDetail>();
            chartDataModels = new List<ChartData>();
            genericResult = new GenericResult();
        }

        public List<StockDataDetail> stockDataDetailModels;
        public List<ChartData> chartDataModels;
        public GenericResult genericResult;
    }
}