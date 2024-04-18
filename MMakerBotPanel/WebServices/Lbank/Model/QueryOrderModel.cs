namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;

    public class QueryOrderModel
    {
        public QueryOrderModel()
        {

            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public string symbol { get; set; }
        public double amount { get; set; }
        public long create_time { get; set; }
        public double price { get; set; }
        public double avg_price { get; set; }
        public string type { get; set; }
        public string order_id { get; set; }
        public double deal_amount { get; set; }
        public int status { get; set; }
        public string customer_id { get; set; }
    }
}