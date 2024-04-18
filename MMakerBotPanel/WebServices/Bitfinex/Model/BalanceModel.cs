namespace MMakerBotPanel.WebServices.Bitfinex.Model
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
        public List<List<object>> Wallets { get; set; }
    }
}