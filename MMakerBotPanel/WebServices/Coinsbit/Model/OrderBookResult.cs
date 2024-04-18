namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    using System.Collections.Generic;

    public class OrderBookResult
    {
        public int offset { get; set; }
        public int limit { get; set; }
        public int total { get; set; }
        public List<Order> orders { get; set; }
    }
}