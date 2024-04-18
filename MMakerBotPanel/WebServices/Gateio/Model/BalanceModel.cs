namespace MMakerBotPanel.WebServices.Gateio.Model
{
    using MMakerBotPanel.Models;

    public class BalanceModel
    {
        public BalanceModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public Details details { get; set; }
    }
}