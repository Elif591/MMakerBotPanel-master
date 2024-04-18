namespace MMakerBotPanel.Business.Strategy.CexMaker.Model
{
    public class CexMakerParameterModel
    {
        public CexMakerParameterModel()
        {

        }
        public double deposit { get; set; }
        public string exchange { get; set; }
        public string symbol { get; set; }
        public int workerID { get; set; }
        public int userID { get; set; }
        public double takeProfit { get; set; }
        public double stopLass { get; set; }
        public double comissionBuy { get; set; }
        public double comissionSell { get; set; }
        public string qtyFormat { get; set; }
    }
}