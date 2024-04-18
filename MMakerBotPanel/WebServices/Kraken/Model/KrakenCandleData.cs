namespace MMakerBotPanel.WebServices.Kraken.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class KrakenCandleData
    {
        public KrakenCandleData()
        {
            genericResult = new GenericResult();
        }
        public List<object> error { get; set; }
        public Result result { get; set; }
        public GenericResult genericResult;
    }


}