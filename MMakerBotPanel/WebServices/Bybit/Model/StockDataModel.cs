namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StockDataModel
    {
        public StockDataModel()
        {
            genericResult = new GenericResult();
            BybitDataDetailModels = new BybitDataDetail();
            BybitDataChartModels = new List<BybitDataChart>();
        }

        public GenericResult genericResult;
        public BybitDataDetail BybitDataDetailModels;
        public List<BybitDataChart> BybitDataChartModels;
    }
}