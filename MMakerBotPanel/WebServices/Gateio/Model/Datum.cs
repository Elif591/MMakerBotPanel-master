namespace MMakerBotPanel.WebServices.Gateio.Model
{
    public class Datum
    {
        public string id { get; set; }
        public string @base { get; set; }
        public string quote { get; set; }
        public string fee { get; set; }
        public string min_quote_amount { get; set; }
        public int amount_precision { get; set; }
        public int precision { get; set; }
        public string trade_status { get; set; }
        public int sell_start { get; set; }
        public int buy_start { get; set; }
        public string min_base_amount { get; set; }
    }
}