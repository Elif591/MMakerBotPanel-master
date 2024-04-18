namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using MMakerBotPanel.Models;
    public class BalanceModel
    {
        public BalanceModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public string status { get; set; }
        public GetWalletData data { get; set; }
    }
}