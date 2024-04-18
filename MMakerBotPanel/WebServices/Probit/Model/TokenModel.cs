namespace MMakerBotPanel.WebServices.Probit.Model
{
    using MMakerBotPanel.Models;

    public class TokenModel
    {
        public TokenModel() 
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
    }
}