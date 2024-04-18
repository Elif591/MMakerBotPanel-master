namespace MMakerBotPanel.WebServices.Alterdice.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ListTrade
    {
        public string id { get; set; }
        public string volume { get; set; }
        public string price { get; set; }
        public int time_create { get; set; }
        public string rate { get; set; }
        public int commission { get; set; }
    }
}