namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    using System.Collections.Generic;

    public class CoinsbitSymbolData
    {
        public int code { get; set; }
        public bool success { get; set; }
        public string message { get; set; }
        public List<string> result { get; set; }
    }
}