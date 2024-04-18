namespace MMakerBotPanel.WebServices.Kraken.Model
{
    using System.Collections.Generic;

    public class KrakenSymbol
    {
        public string altname { get; set; }
        public string wsname { get; set; }
        public string aclass_base { get; set; }
        public string @base { get; set; }
        public string aclass_quote { get; set; }
        public string quote { get; set; }
        public string lot { get; set; }
        public int cost_decimals { get; set; }
        public int pair_decimals { get; set; }
        public int lot_decimals { get; set; }
        public int lot_multiplier { get; set; }
        public List<object> leverage_buy { get; set; }
        public List<object> leverage_sell { get; set; }
        public List<List<double>> fees { get; set; }
        public List<List<double>> fees_maker { get; set; }
        public string fee_volume_currency { get; set; }
        public int margin_call { get; set; }
        public int margin_stop { get; set; }
        public string ordermin { get; set; }
        public string costmin { get; set; }
        public string tick_size { get; set; }
        public string status { get; set; }
    }
}