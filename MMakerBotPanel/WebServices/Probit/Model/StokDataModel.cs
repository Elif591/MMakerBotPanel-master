namespace MMakerBotPanel.WebServices.Probit.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class StokDataModel
    {
        public StokDataModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;
        public List<StokData> data { get; set; }
    }
}