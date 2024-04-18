namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StockDataModel
    {
        public StockDataModel()
        {
            genericResult = new GenericResult();
            CoinsbitDataDetailModels = new CoinsbitDataDetail();
            CoinsbitChartDataModels = new List<CoinsbitChartData>();

        }
        public GenericResult genericResult;
        public CoinsbitDataDetail CoinsbitDataDetailModels;
        public List<CoinsbitChartData> CoinsbitChartDataModels;
    }
}