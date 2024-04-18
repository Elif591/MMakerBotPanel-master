namespace MMakerBotPanel.WebServices.Kraken.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class ActiveOrdersModel
    {
        public ActiveOrdersModel() 
        {
            genericResult = new GenericResult();    
        }
        public GenericResult genericResult;
        public List<object> error { get; set; }
        public string result { get; set; }
    }
}