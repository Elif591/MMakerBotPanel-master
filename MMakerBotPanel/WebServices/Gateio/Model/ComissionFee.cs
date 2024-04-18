namespace MMakerBotPanel.WebServices.Gateio.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ComissionFee
    {
        public int total { get; set; }
        public List<ComissionList> list { get; set; }
    }
}