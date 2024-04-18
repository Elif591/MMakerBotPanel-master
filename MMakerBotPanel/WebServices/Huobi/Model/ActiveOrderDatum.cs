namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using Newtonsoft.Json;
    public class ActiveOrderDatum
    {
        public string symbol { get; set; }
        public string source { get; set; }
        public string price { get; set; }

        [JsonProperty("created-at")]
        public long createdat { get; set; }
        public string amount { get; set; }

        [JsonProperty("account-id")]
        public int accountid { get; set; }

        [JsonProperty("filled-cash-amount")]
        public string filledcashamount { get; set; }

        [JsonProperty("client-order-id")]
        public string clientorderid { get; set; }

        [JsonProperty("filled-amount")]
        public string filledamount { get; set; }

        [JsonProperty("filled-fees")]
        public string filledfees { get; set; }
        public long id { get; set; }
        public string state { get; set; }
        public string type { get; set; }
    }
}