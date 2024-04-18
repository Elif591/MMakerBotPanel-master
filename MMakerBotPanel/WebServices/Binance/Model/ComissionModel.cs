namespace MMakerBotPanel.WebServices.Binance.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ComissionModel
    {
        public ComissionModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult;

        public List<ComissionFeeModel> comissionFee { get; set; }
    }
}