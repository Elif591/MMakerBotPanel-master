namespace MMakerBotPanel.WebServices.Probit.Model
{
    public class Symbol
    {
        public string id { get; set; }
        public string base_currency_id { get; set; }
        public string quote_currency_id { get; set; }
        public string min_price { get; set; }
        public string max_price { get; set; }
        public string price_increment { get; set; }
        public string min_quantity { get; set; }
        public string max_quantity { get; set; }
        public int quantity_precision { get; set; }
        public string min_cost { get; set; }
        public string max_cost { get; set; }
        public int cost_precision { get; set; }
        public string taker_fee_rate { get; set; }
        public string maker_fee_rate { get; set; }
        public bool show_in_ui { get; set; }
        public bool closed { get; set; }
    }
}