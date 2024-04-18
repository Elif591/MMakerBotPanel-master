namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class TradeModel
    {
        public TradeModel()
        {
            genericResult = new GenericResult();
            data = new List<TradeDatum>();
        }

        public GenericResult genericResult { get; set; }
        public string ch { get; set; }
        public string status { get; set; }
        public long ts { get; set; }
        public List<TradeDatum> data { get; set; }
    }
}