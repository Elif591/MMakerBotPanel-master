namespace MMakerBotPanel.WebServices.Binance.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;

    public class CancelOrderModel
    {
        public CancelOrderModel()
        {
            genericResult = new GenericResult();

        }
        public GenericResult genericResult;

        public List<CancelOrderListModel> cancelOrderListModel { get; set; }

    }
}