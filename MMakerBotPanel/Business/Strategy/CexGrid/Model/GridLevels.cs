namespace MMakerBotPanel.Business.Strategy.CexGrid.Model
{
    using MMakerBotPanel.Models;

    public class GridLevels
    {
        public string symbol { get; set; }
        public SIDE? side { get; set; }
        public TYPE_TRADE? typeTrade { get; set; }
        public string amount { get; set; }
        public string price { get; set; }
        public string orderId { get; set; }
    }
}