namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class TradeModel
    {
        public TradeModel()
        {
            genericResult = new GenericResult();
            data = new List<TradesData>();
        }
        public GenericResult genericResult { get; set; }
        public string result { get; set; }
        public int error_code { get; set; }
        public long ts { get; set; }
        public List<TradesData> data { get; set; }
    }
}