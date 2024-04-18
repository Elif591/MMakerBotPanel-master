namespace MMakerBotPanel.WebServices.Bybit.Model
{
    using MMakerBotPanel.Models;

    public class StatusModel
    {
        public StatusModel()
        {

            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public int retCode { get; set; }
        public string retMsg { get; set; }
        public ResultStatus result { get; set; }
        public RetExtInfo retExtInfo { get; set; }
        public long time { get; set; }
    }
}