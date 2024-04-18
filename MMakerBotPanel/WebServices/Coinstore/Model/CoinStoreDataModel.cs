namespace MMakerBotPanel.WebServices.Coinstore.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class CoinStoreDataModel
    {
        public CoinStoreDataModel()
        {
            genericResult = new GenericResult();
            CoinStoreDataDetailModels = new CoinStoreDataDetailModel();
            CoinStoreChartDataModels = new List<CoinStoreChartDataModel>();

        }
        public GenericResult genericResult;
        public CoinStoreDataDetailModel CoinStoreDataDetailModels;
        public List<CoinStoreChartDataModel> CoinStoreChartDataModels;
    }
}