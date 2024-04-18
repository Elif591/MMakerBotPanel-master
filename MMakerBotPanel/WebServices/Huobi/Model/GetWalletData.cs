namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using System.Collections.Generic;

    public class GetWalletData
    {
        public int id { get; set; }
        public string type { get; set; }
        public string state { get; set; }
        public List<BalanceList> list { get; set; }
    }
}