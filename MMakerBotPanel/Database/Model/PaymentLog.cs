using MMakerBotPanel.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMakerBotPanel.Database.Model
{
    [Table("PaymentLog", Schema = "Log")]
    public class PaymentLog
    {
        [Key]
        public int PaymentLogID { get; set; }
        public string WalletID { get; set; }
        public string AdminWalletID { get; set; }
        public string BrowserInfo { get; set; }
        public string IPNumber { get; set; }
        public string ProductID { get; set; }
        public string WorkerID { get; set; }
        public string Price { get; set; }
        public string PriceUnitID { get; set; }
        public string GasFee { get; set; }
        public Int32 Timestamp { get; set; }
        public int UserID { get; set; }
    }
}