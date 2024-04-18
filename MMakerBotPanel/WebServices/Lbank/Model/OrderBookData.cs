namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using System.Collections.Generic;
    public class OrderBookData
    {
        public OrderBookData()
        {
            asks = new List<List<double>>();
            bids = new List<List<double>>();
        }
        public List<List<double>> asks { get; set; }
        public List<List<double>> bids { get; set; }
    }

}