namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using Newtonsoft.Json;
    public class QueryOrder
    {
        public string id { get; set; }
        public string symbol { get; set; }

        [JsonProperty("account-id")]
        public int accountid { get; set; }

        [JsonProperty("client-order-id")]
        public string clientorderid { get; set; }
        public string amount { get; set; }
        public string price { get; set; }

        [JsonProperty("created-at")]
        public long createdat { get; set; }
        public string type { get; set; }

        [JsonProperty("field-amount")]
        public string fieldamount { get; set; }

        [JsonProperty("field-cash-amount")]
        public string fieldcashamount { get; set; }

        [JsonProperty("field-fees")]
        public string fieldfees { get; set; }

        [JsonProperty("finished-at")]
        public int finishedat { get; set; }
        public string source { get; set; }
        public string state { get; set; }

        [JsonProperty("canceled-at")]
        public int canceledat { get; set; }
    }
}