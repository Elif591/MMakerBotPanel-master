namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Worker", Schema = "Worker")]
    public class Worker
    {
        [Key]
        public int WorkerID { get; set; }

        [DisplayName("Worker Name")]
        public string WorkerName { get; set; }

        [DisplayName("Starting Date")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime StartingDate { get; set; }

        [DisplayName("Ending Date")]
        [Column(TypeName = "datetime2")]
        public DateTime EndingDate { get; set; }

        [DisplayName("State")]
        public WORKERSTATE WorkerState { get; set; }


        //////////////////////////////////

        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }

        [ForeignKey("WorkerDetail")]
        public int? WorkerDetayID { get; set; }
        public virtual WorkerDetail WorkerDetail { get; set; }

        [ForeignKey("Product")]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }


        [ForeignKey("GridParameter")]
        public int? GridParameterID { get; set; }
        public virtual GridParameter GridParameter { get; set; }

        [ForeignKey("MakerParameter")]
        public int? MakerParameterID { get; set; }
        public virtual MakerParameter MakerParameter { get; set; }


        public virtual ICollection<PurchaseHistory> PurchaseHistories { get; set; }

        public virtual ICollection<GridParameter> GridParameters { get; set; }

        public virtual ICollection<MakerParameter> MakerParameters { get; set; }

        //////////////////////////////////

        [DisplayName("Days Remaining")]
        [NotMapped]
        public int DaysRemaining { get; set; }
        [NotMapped]
        public int HoursRemaining { get; set; }
    }
}