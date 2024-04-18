namespace MMakerBotPanel.WebServices.OKX.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class TradeModel
    {
        public TradeModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public string code { get; set; }
        public string msg { get; set; }
        public List<TradeDatum> data { get; set; }
    }
}