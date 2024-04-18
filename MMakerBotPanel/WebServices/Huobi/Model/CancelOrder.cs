namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using Newtonsoft.Json;

    public class CancelOrder
    {
        [JsonProperty("success-count")]
        public int successcount { get; set; }

        [JsonProperty("failed-count")]
        public int failedcount { get; set; }

        [JsonProperty("next-id")]
        public int nextid { get; set; }
    }
}