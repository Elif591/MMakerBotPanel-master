namespace MMakerBotPanel.WebServices.Kraken.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class KrakenExchange
    {
        public KrakenExchange()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public List<object> error { get; set; }
        public ResultExchange result { get; set; }
    }
}