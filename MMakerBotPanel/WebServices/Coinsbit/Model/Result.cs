namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    using System.Collections.Generic;

    public class Result
    {
        public string market { get; set; }
        public int start { get; set; }
        public int end { get; set; }
        public int interval { get; set; }
        public List<Kline> kline { get; set; }
    }
}