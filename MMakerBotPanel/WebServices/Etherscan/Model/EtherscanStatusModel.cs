namespace MMakerBotPanel.WebServices.Etherscan.Model
{
    using MMakerBotPanel.Models;

    public class EtherscanStatusModel
    {
        public EtherscanStatusModel()
        {
            genericResult = new GenericResult();

        }
        public GenericResult genericResult;
        public string status { get; set; }
        public string message { get; set; }
        public Result result { get; set; }
    }
}