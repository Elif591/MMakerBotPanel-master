namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    using System.Collections.Generic;

    public class ActiveOrderResult
    {
        public int limit { get; set; }
        public int offset { get; set; }
        public int total { get; set; }
        public List<ActiveOrderResult> result { get; set; }
        public int id { get; set; }
        public string left { get; set; }
        public string market { get; set; }
        public string amount { get; set; }
        public string type { get; set; }
        public string price { get; set; }
        public double timestamp { get; set; }
        public string side { get; set; }
        public string dealFee { get; set; }
        public string takerFee { get; set; }
        public string makerFee { get; set; }
        public string dealStock { get; set; }
        public string dealMoney { get; set; }
    }
}