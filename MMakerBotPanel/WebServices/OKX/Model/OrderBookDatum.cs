namespace MMakerBotPanel.WebServices.OKX.Model
{
    using System.Collections.Generic;

    public class OrderBookDatum
    {
        public OrderBookDatum()
        {
            asks = new List<List<string>>();
            bids = new List<List<string>>();
        }
        public List<List<string>> asks { get; set; }
        public List<List<string>> bids { get; set; }
        public string ts { get; set; }
    }
}