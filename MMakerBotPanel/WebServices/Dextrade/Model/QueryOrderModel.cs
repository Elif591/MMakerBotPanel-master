namespace MMakerBotPanel.WebServices.Dextrade.Model
{
    using MMakerBotPanel.Models;

    public class QueryOrderModel
    {
        public QueryOrderModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public bool status { get; set; }
        public QueryOrderData data { get; set; }
    }
}