namespace MMakerBotPanel.WebServices.Bitfinex.Model
{
    using System.Text.Json.Serialization;

    public class Order
    {
        public int ID { get; set; }
        public int GID { get; set; }
        public int CID { get; set; }
        public string SYMBOL { get; set; }
        public int MTS_CREATE { get; set; }
        public int MTS_UPDATE { get; set; }
        public float AMOUNT { get; set; }
        public int AMOUNT_ORIG { get; set; }
        public string ORDER_TYPE { get; set; }
        public string TYPE_PREV { get; set; }
        public int MTS_TIF { get; set; }
        public int FLAGS { get; set; }
        public string STATUS { get; set; }
        public float PRICE { get; set; }
        public float PRICE_AVG { get; set; }
        public float PRICE_TRAILING { get; set; }
        public float PRICE_AUX_LIMIT { get; set; }
        public int NOTIFY { get; set; }
        public int HIDDEN { get; set; }
        public int PLACED_ID { get; set; }
        public string ROUTING { get; set; }
        public JsonAttribute META { get; set; }
    }
}