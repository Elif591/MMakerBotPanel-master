namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;

    public class TimeModel
    {

        public TimeModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public string result { get; set; }
        public long data { get; set; }
        public int error_code { get; set; }
        public long ts { get; set; }

    }
}