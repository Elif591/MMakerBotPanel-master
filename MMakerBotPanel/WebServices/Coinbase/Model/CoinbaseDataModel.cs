namespace MMakerBotPanel.WebServices.Coinbase.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class CoinbaseDataModel
    {
        public CoinbaseDataModel()
        {
            genericResult = new GenericResult();
            CoinbaseDatDetailaModels = new CoinbaseDatDetailaModel();
            CoinbaseChartDataModels = new List<CoinbaseChartDataModel>();
        }
        public GenericResult genericResult;
        public CoinbaseDatDetailaModel CoinbaseDatDetailaModels;
        public List<CoinbaseChartDataModel> CoinbaseChartDataModels;
    }
}