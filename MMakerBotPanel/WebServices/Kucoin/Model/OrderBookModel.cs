namespace MMakerBotPanel.WebServices.Kucoin.Model
{
    using MMakerBotPanel.Models;

    public class OrderBookModel
    {
        public OrderBookModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public string sequence { get; set; }
        public long time { get; set; }
        public OrderBookData data { get; set; }

    }
}