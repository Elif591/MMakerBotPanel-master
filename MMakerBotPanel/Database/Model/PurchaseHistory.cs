namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PurchaseHistory", Schema = "General")]
    public class PurchaseHistory
    {
        [Key]
        public int PurchaseHistoryID { get; set; }
        public int Timestamp { get; set; }
        public string Amount { get; set; }
        public string Contract { get; set; }
        public string ShortNmae { get; set; }
        public PURCHASESTATUS PurchaseStatusType { get; set; }
        public string TransectionId { get; set; }
        public string ErrorMessage { get; set; }
        public bool Monthly { get; set; }
        public bool Yearly { get; set; }
        public string AccountWallet { get; set; }
        public string ReceiverWallet { get; set; }
        public int TtlCount { get; set; }
        public int LastCheckDate { get; set; }



        ////////////////////////////////

        [ForeignKey("Worker")]
        public int WorkerID { get; set; }
        public virtual Worker Worker { get; set; }

        [ForeignKey("PriceUnit")]
        public int PriceUnitID { get; set; }
        public virtual PriceUnit PriceUnit { get; set; }



    }
}