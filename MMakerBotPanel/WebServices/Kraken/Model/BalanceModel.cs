namespace MMakerBotPanel.WebServices.Kraken.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class BalanceModel
    {
        public BalanceModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public List<object> error { get; set; }
        public string result { get; set; }
    }
}