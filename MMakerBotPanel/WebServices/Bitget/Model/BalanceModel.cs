namespace MMakerBotPanel.WebServices.Bitget.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class BalanceModel
    {
        public BalanceModel() 
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;

        public string code { get; set; }
        public string message { get; set; }
        public List<Balance> data { get; set; }
    }
}