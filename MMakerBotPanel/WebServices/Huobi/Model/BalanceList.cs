namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using Newtonsoft.Json;
    public class BalanceList
    {
        public string currency { get; set; }
        public string type { get; set; }
        public string balance { get; set; }

        [JsonProperty("seq-num")]
        public string seqnum { get; set; }
    }
}