namespace MMakerBotPanel.WebServices.Binance.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class OrderBookModel
    {
        public OrderBookModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public long lastUpdateId { get; set; }
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }
    }
}