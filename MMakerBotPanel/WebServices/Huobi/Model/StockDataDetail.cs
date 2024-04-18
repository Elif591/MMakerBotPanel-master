namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using System.Collections.Generic;

    public class StockDataDetail
    {
        public string ch { get; set; }
        public string status { get; set; }
        public long ts { get; set; }
        public List<Data> data { get; set; }
    }
}