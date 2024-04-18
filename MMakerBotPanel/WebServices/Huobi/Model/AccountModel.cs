namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class AccountModel
    {
        public AccountModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public string status { get; set; }
        public List<AccountData> data { get; set; }
    }

}