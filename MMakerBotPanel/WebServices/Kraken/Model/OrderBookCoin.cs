namespace MMakerBotPanel.WebServices.Kraken.Model
{
    using System.Collections.Generic;
    public class OrderBookCoin
    {
        public Dictionary<string, Asks> Asks { get; set; }
        public Dictionary<string, Bids> Bids { get; set; }
    }
}