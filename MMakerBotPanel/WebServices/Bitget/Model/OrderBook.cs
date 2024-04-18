namespace MMakerBotPanel.WebServices.Bitget.Model
{
    using System.Collections.Generic;

    public class OrderBook
    {
        public List<List<string>> asks { get; set; }
        public List<List<string>> bids { get; set; }
        public string timestamp { get; set; }
    }
}