namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StatusModel
    {
        public StatusModel()
        {
            genericResult = new GenericResult();
            coinsbitStatusModels = new List<CoinsbitStatus>();
        }
        public GenericResult genericResult;
        public List<CoinsbitStatus> coinsbitStatusModels;
    }
}