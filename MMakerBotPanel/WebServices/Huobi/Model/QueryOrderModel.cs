namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using MMakerBotPanel.Models;

    public class QueryOrderModel
    {
        public QueryOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }

        public string status { get; set; }
        public QueryOrder data { get; set; }
    }
}