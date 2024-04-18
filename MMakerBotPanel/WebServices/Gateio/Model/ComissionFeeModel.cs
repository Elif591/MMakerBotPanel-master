namespace MMakerBotPanel.WebServices.Gateio.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;

    public class ComissionFeeModel
    {
        public ComissionFeeModel()
        {
            comissionFee = new List<ComissionFee>();
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public List<ComissionFee> comissionFee { get; set; }


    }
}