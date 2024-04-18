namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using MMakerBotPanel.Models;

    public class OrderBookModel
    {
        public OrderBookModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public int retCode { get; set; }
        public string retMsg { get; set; }
        public OrderBookResult result { get; set; }
        public RetExtInfo retExtInfo { get; set; }
        public long time { get; set; }
    }
}