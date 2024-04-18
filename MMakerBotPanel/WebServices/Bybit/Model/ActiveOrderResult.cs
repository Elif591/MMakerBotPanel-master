namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using System.Collections.Generic;

    public class ActiveOrderResult
    {
        public string nextPageCursor { get; set; }
        public string category { get; set; }
        public List<ActiveOrderList> list { get; set; }
    }
}