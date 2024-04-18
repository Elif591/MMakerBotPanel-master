namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using System.Collections.Generic;

    public class Tick
    {
        public long ts { get; set; }
        public long version { get; set; }
        public List<List<double>> bids { get; set; }
        public List<List<double>> asks { get; set; }
    }
}