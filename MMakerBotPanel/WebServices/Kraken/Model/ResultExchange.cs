namespace MMakerBotPanel.WebServices.Kraken.Model
{
    using System.Collections.Generic;

    public class ResultExchange
    {
        public Descr descr { get; set; }
        public List<string> txid { get; set; }
    }
}