namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    using MMakerBotPanel.Models;

    public class OrderBookModel
    {
        public OrderBookModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }

        public long code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public OrderBookResult result { get; set; }
    }
}