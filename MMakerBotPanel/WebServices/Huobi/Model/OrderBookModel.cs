namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using MMakerBotPanel.Models;
    public class OrderBookModel
    {
        public OrderBookModel()
        {
            genericResult = new GenericResult();
        }

        public GenericResult genericResult { get; set; }
        public string ch { get; set; }
        public string status { get; set; }
        public long ts { get; set; }
        public Tick tick { get; set; }
    }
}