namespace MMakerBotPanel.WebServices.Coinsbit.Model
{
    public class CancelOrderResult
    {
        public int orderId { get; set; }
        public string market { get; set; }
        public string price { get; set; }
        public string side { get; set; }
        public string type { get; set; }
        public double timestamp { get; set; }
        public string dealMoney { get; set; }
        public string dealStock { get; set; }
        public string amount { get; set; }
        public string takerFee { get; set; }
        public string makerFee { get; set; }
        public string left { get; set; }
        public string dealFee { get; set; }
    }
}