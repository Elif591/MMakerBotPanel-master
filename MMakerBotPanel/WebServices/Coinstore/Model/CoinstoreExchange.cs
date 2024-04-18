namespace MMakerBotPanel.WebServices.Coinstore.Model
{
    using MMakerBotPanel.Models;

    public class CoinstoreExchange
    {
        public CoinstoreExchange()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }

        public string code { get; set; }
        public DataExchange data { get; set; }


    }
}