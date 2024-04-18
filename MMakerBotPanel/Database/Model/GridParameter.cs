namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("GridParameter", Schema = "Worker")]
    public class GridParameter
    {
        [Key]
        public int GridParameterID { get; set; }
        public int userID { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public double TakeProfit { get; set; }
        public double StopLoss { get; set; }
        public int GridCount { get; set; }
        public double Deposit { get; set; }
        public string MinProfitPerGrid { get; set; }
        public string MaxProfitPerGrid { get; set; }
        public string CurrentUSDT { get; set; }
        public CEXEXCHANGE Exchange { get; set; }
        public string Parity { get; set; }
        public ParameterTYPE ParameterType { get; set; }
        public BOTSTATUS  gridStatus { get; set; }

        //////////////////////////////

        [ForeignKey("Worker")]
        public int WorkerID { get; set; }
        public virtual Worker Worker { get; set; }
    }
}