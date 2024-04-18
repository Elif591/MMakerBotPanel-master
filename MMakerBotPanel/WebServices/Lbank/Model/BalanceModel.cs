namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class BalanceModel
    {
        public BalanceModel()
        {

            genericResult = new GenericResult();
            balances = new List<SpotBalances>();
        }
        public GenericResult genericResult;
        public List<SpotBalances> balances;
    }
}