namespace MMakerBotPanel.Models.MakerMarketingModels
{
    using System.Collections.Generic;

    public class OrderBookResponseModel
    {
        public OrderBookResponseModel()
        {
            GenericResult = new GenericResult();
            Buy = new List<OrderBookBuy>();
            Sell = new List<OrderBookSell>();
        }
        public GenericResult GenericResult { get; set; }
        public List<OrderBookBuy> Buy;
        public List<OrderBookSell> Sell;
    }
}