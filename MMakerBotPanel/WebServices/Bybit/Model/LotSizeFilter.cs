namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class LotSizeFilter
    {
        public string basePrecision { get; set; }
        public string quotePrecision { get; set; }
        public string minOrderQty { get; set; }
        public string maxOrderQty { get; set; }
        public string minOrderAmt { get; set; }
        public string maxOrderAmt { get; set; }
    }
}