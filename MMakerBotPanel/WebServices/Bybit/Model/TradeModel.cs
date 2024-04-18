namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using MMakerBotPanel.Models;

    public class TradeModel
    {
        public TradeModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public int retCode { get; set; }
        public string retMsg { get; set; }
        public TradeResult result { get; set; }
        public RetExtInfo retExtInfo { get; set; }
        public long time { get; set; }
    }
}