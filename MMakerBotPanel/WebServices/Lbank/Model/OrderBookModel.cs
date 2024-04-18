namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;

    public class OrderBookModel
    {
        public OrderBookModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public OrderBookData data { get; set; }

    }
}