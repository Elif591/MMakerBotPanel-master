namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using System.Collections.Generic;

    public class StockDataDetail
    {
        public string result { get; set; }
        public List<List<double>> data { get; set; }
        public int error_code { get; set; }
        public long ts { get; set; }
    }
}