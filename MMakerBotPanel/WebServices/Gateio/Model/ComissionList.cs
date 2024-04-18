namespace MMakerBotPanel.WebServices.Gateio.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ComissionList
    {
        public int commission_time { get; set; }
        public int user_id { get; set; }
        public string group_name { get; set; }
        public string commission_amount { get; set; }
        public string source { get; set; }
        public string commission_asset { get; set; }
    }
}