namespace MMakerBotPanel.WebServices.Gateio.Model
{
    public class TradesData
    {
        public string id { get; set; }
        public string create_time { get; set; }
        public string create_time_ms { get; set; }
        public string order_id { get; set; }
        public string side { get; set; }
        public string role { get; set; }
        public string amount { get; set; }
        public string price { get; set; }
        public string fee { get; set; }
        public string fee_currency { get; set; }
        public string point_fee { get; set; }
        public string gt_fee { get; set; }
    }
}