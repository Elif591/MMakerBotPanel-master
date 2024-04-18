namespace MMakerBotPanel.WebServices.Dextrade.Model
{
    public class GetWalletList
    {
        public int balance { get; set; }
        public int balance_available { get; set; }
        public Balances balances { get; set; }
        public int @decimal { get; set; }
        public Currency currency { get; set; }
    }
}