namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using System.Collections.Generic;

    public class OrderBookResult
    {
        public string s { get; set; }
        public List<List<string>> a { get; set; }
        public List<List<string>> b { get; set; }
        public long ts { get; set; }
        public int u { get; set; }
    }
}