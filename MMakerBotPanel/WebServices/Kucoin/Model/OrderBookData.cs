namespace MMakerBotPanel.WebServices.Kucoin.Model
{
    using System.Collections.Generic;
    public class OrderBookData
    {
        public OrderBookData()
        {
            bids = new List<List<string>>();
            asks = new List<List<string>>();
        }
        public List<List<string>> bids { get; set; }
        public List<List<string>> asks { get; set; }
    }
}