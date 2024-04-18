namespace MMakerBotPanel.WebServices.Bitget.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class QueryOrderModel
    {
        public QueryOrderModel() 
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public string code { get; set; }
        public string msg { get; set; }
        public long requestTime { get; set; }
        public List<QueryOrder> data { get; set; }
    }
}