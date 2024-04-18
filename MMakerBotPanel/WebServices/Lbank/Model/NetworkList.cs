namespace MMakerBotPanel.WebServices.Lbank.Model
{
    public class NetworkList
    {
        public bool isDefault { get; set; }
        public string withdrawFeeRate { get; set; }
        public string name { get; set; }
        public double withdrawMin { get; set; }
        public double minLimit { get; set; }
        public double minDeposit { get; set; }
        public string feeAssetCode { get; set; }
        public string withdrawFee { get; set; }
        public int type { get; set; }
        public string coin { get; set; }
        public string network { get; set; }
    }
}