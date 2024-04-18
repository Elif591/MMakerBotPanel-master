﻿namespace MMakerBotPanel.WebServices.OKX.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class QueryOrderModel
    {
        public QueryOrderModel() 
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public string code { get; set; }
        public string msg { get; set; }
        public List<QueryOrders> data { get; set; }
    }
}