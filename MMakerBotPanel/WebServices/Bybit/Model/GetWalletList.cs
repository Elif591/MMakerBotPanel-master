namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using System.Collections.Generic;

    public class GetWalletList
    {
        public string totalEquity { get; set; }
        public string accountIMRate { get; set; }
        public string totalMarginBalance { get; set; }
        public string totalInitialMargin { get; set; }
        public string accountType { get; set; }
        public string totalAvailableBalance { get; set; }
        public string accountMMRate { get; set; }
        public string totalPerpUPL { get; set; }
        public string totalWalletBalance { get; set; }
        public string accountLTV { get; set; }
        public string totalMaintenanceMargin { get; set; }
        public List<Coin> coin { get; set; }
    }
}