namespace MMakerBotPanel.Database.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Web;

    [Table("ProfitUSDT", Schema = "General")]
    public class ProfitUSDT
    {
        public int ProfitUSDTID { get; set; }
        public double UsdtBalance { get; set; }
        public DateTime Date { get; set; }
        public int UserID { get; set; }
    }
}