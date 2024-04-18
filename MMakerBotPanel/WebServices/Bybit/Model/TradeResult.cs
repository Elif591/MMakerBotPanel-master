namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using System.Collections.Generic;

    public class TradeResult
    {
        public string category { get; set; }
        public List<TradeList> list { get; set; }
    }
}