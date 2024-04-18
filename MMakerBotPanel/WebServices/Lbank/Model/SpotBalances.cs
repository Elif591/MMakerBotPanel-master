namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using System.Collections.Generic;

    public class SpotBalances
    {
        public string usableAmt { get; set; }
        public string assetAmt { get; set; }
        public List<NetworkList> networkList { get; set; }
        public string freezeAmt { get; set; }
        public string coin { get; set; }
    }
}