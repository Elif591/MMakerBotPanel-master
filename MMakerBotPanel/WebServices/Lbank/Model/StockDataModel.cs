namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StockDataModel
    {
        public StockDataModel()
        {
            genericResult = new GenericResult();
            LbankDataDetailModels = new StockDataDetail();
            LbankChartDataModels = new List<ChartData>();

        }
        public GenericResult genericResult;
        public StockDataDetail LbankDataDetailModels;
        public List<ChartData> LbankChartDataModels;
    }

}