namespace MMakerBotPanel.Models.CexBotModels
{
    using System;

    public class CexBotStatusModel
    {
        public CexBotStatusModel()
        {
            CexBotStatusModels = new CexBotStatus();
            genericResult = new GenericResult();
        }
        public CexBotStatus CexBotStatusModels;
        public GenericResult genericResult;
    }

    public class CexBotStatus
    {
        public bool status { get; set; }
        public DateTime date { get; set; }

    }

}