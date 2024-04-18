namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("WorkerDetail", Schema = "Worker")]
    public class WorkerDetail
    {
        [Key]
        [ForeignKey("Worker")]
        public int WorkerDetayID { get; set; }

        [DisplayName("Exchange Type")]
        public EXCHANGETYPE ExchangeType { get; set; }

        [DisplayName("Exchange")]
        public CEXEXCHANGE CexExchange { get; set; }

        [DisplayName("Exchange")]
        public DEXEXCHANGE? DexExchange { get; set; }

        public string Parity { get; set; }


        /////////////////////////////

        public virtual Worker Worker { get; set; }
    }
}
