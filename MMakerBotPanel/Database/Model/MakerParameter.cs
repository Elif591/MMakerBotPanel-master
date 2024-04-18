namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.ComponentModel.DataAnnotations;


    [Table("MakerParameter", Schema = "Worker")]
    public class MakerParameter
    {
        [Key]
        public int MakerParameterID { get; set; }
        public int userID { get; set; }
        public RiskLevel riskLevel { get; set; }    
        public double TakeProfit { get; set; }
        public double StopLoss { get; set; }
        public double Deposit { get; set; }
        public string CurrentUSDT { get; set; }
        public CEXEXCHANGE Exchange { get; set; }
        public string Parity { get; set; }
        public ParameterTYPE ParameterType { get; set; }
        public BOTSTATUS makerStatus { get; set; }

        //////////////////////////////

        [ForeignKey("Worker")]
        public int WorkerID { get; set; }
        public virtual Worker Worker { get; set; }
    }

}