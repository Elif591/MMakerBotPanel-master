namespace MMakerBotPanel.WebServices.Dextrade.Model
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
        public bool status { get; set; }
        public List<TradesData> data { get; set; }
    }
}