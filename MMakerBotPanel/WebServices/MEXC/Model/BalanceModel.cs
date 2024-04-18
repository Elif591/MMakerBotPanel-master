namespace MMakerBotPanel.WebServices.MEXC.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class BalanceModel
    {
        public BalanceModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public int makerCommission { get; set; }
        public int takerCommission { get; set; }
        public int buyerCommission { get; set; }
        public int sellerCommission { get; set; }
        public bool canTrade { get; set; }
        public bool canWithdraw { get; set; }
        public bool canDeposit { get; set; }
        public object updateTime { get; set; }
        public string accountType { get; set; }
        public List<Balance> balances { get; set; }
        public List<string> permissions { get; set; }
    }
}