namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class QueryOrderResult
    {
        public List<QueryOrderList> list { get; set; }
        public string nextPageCursor { get; set; }
        public string category { get; set; }
    }
}