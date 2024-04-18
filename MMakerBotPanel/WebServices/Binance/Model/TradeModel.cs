namespace MMakerBotPanel.WebServices.Binance.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class TradeModel
    {
        public TradeModel()
        {
            genericResult = new GenericResult();
            trades = new List<TradesData>();
        }
        public GenericResult genericResult { get; set; }
        public List<TradesData> trades { get; set; }
    }
}