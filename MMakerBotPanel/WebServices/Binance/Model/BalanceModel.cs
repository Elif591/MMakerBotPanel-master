namespace MMakerBotPanel.WebServices.Binance.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class BalanceModel
    {

        public BalanceModel()
        {
            genericResult = new GenericResult();
            getWallets = new List<BalanceData>();
        }
        public GenericResult genericResult;
        public List<BalanceData> getWallets;

    }
}