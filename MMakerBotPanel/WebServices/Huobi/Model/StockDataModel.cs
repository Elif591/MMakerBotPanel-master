namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StockDataModel
    {
        public StockDataModel()
        {
            genericResult = new GenericResult();
            HuobiChartDataDetailModels = new StockDataDetail();
            HuobiChartModels = new List<ChartData>();
        }
        public GenericResult genericResult;
        public StockDataDetail HuobiChartDataDetailModels;
        public List<ChartData> HuobiChartModels;
    }
}