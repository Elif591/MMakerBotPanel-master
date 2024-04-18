namespace MMakerBotPanel.WebServices.Dextrade.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StockDataModel
    {
        public StockDataModel()
        {
            genericResult = new GenericResult();
            Dex_TradeDataModels = new List<DataDetail>();
            Dex_TradeChartDataModels = new List<ChartData>();
        }
        public GenericResult genericResult;
        public List<DataDetail> Dex_TradeDataModels;
        public List<ChartData> Dex_TradeChartDataModels;
    }
}