namespace MMakerBotPanel.Models.MakerMarketingModels
{

    public class CreateOrderRequestModel
    {
        public CreateOrderRequestModel() 
        {
        }

        public string symbol { get; set; }
        public SIDE? side { get; set; }
        public TYPE_TRADE? typeTrade { get; set; }
        public string amount { get; set; }
        public string price { get; set; }
        public string orderId { get; set; }    
    }
}