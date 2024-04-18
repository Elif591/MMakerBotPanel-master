namespace MMakerBotPanel.WebServices.Dextrade.Model
{
    using System.Collections.Generic;

    public class OrderBookData
    {
        public OrderBookData()
        {
            buy = new List<Buy>();
        }
        public List<Buy> buy { get; set; }
    }
}