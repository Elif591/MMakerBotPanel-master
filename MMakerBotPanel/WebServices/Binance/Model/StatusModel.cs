namespace MMakerBotPanel.WebServices.Binance.Model
{
    using MMakerBotPanel.Models;

    public class StatusModel
    {
        public StatusModel()
        {
            genericResult = new GenericResult();
        }

        public int status { get; set; }
        public string msg { get; set; }
        public long serverTime { get; set; }

        public GenericResult genericResult;
    }
}