namespace MMakerBotPanel.WebServices.Coinsbit
{
    using Newtonsoft.Json;

    internal class CoinBlob
    {
        [JsonProperty(PropertyName = "available")]
        public int available;
        [JsonProperty(PropertyName = "freeze")]
        public string freeze;
    }
}