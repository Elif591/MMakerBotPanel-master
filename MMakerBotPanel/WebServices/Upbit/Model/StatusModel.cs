namespace MMakerBotPanel.WebServices.Upbit.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class StatusModel
    {

        public StatusModel()
        {
            genericResult = new GenericResult();
            statusUpbitModels = new List<StatusUpbitModel>();
        }
        public GenericResult genericResult;
        public List<StatusUpbitModel> statusUpbitModels;

    }
}