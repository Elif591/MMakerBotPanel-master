namespace MMakerBotPanel.WebServices.Binance.Model
{
    using MMakerBotPanel.Models;

    public class TimeModel
    {
        public TimeModel()
        {

            genericResult = new GenericResult();
        }

        public GenericResult genericResult;
        public long serverTime { get; set; }
    }

}