namespace MMakerBotPanel.WebServices.MEXC.Model
{
    using MMakerBotPanel.Models;

    public class MEXCExchange
    {
        public MEXCExchange()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }

        public int code { get; set; }
        public string data { get; set; }
    }
}