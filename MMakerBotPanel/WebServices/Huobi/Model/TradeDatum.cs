namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class TradeDatum
    {
        public TradeDatum()
        {
            data = new List<TradeDatum>();
        }
        public object id { get; set; }
        public object ts { get; set; }
        public List<TradeDatum> data { get; set; }

        [JsonProperty("trade-id")]
        public object tradeid { get; set; }
        public double amount { get; set; }
        public double price { get; set; }
        public string direction { get; set; }
    }
}