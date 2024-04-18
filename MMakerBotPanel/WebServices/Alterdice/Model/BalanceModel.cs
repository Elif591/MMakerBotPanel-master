namespace MMakerBotPanel.WebServices.Alterdice.Model
{
    using MMakerBotPanel.Models;

    public class BalanceModel
    {
        public BalanceModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public bool status { get; set; }
        public DataBalance data { get; set; }
        public string token { get; set; }

    }
}