namespace MMakerBotPanel.Business.Strategy.CexGrid.Model
{
    using System.Collections.Generic;

    public class CexGridParameterModel
    {
        public CexGridParameterModel() 
        {
            gridLevel = new List<GridLevels>();
        }

        public List<GridLevels> gridLevel { get; set; }
        public decimal maxPrice { get; set; }
        public int gridCount { get; set; }
        public decimal minPrice { get; set; }
        public double deposit { get; set; }
        public string exchange { get; set; }
        public string symbol { get; set; }
        public int  workerID { get; set; }
        public int  userID { get; set; }
        public double  takeProfit { get; set; }
        public double  stopLass { get; set; }
        public double comissionBuy { get; set; }
        public double comissionSell { get; set; }
        public string qtyFormat { get; set; }


    }
}