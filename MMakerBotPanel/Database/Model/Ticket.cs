namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Ticket", Schema = "Ticket")]
    public class Ticket
    {

        [Key]
        public int TicketID { get; set; }

        [DisplayName("Ticket Name")]
        [MaxLength(150)]
        public string TicketName { get; set; }

        [DisplayName("Ticket Text")]
        [MaxLength(1000)]
        public string TicketText { get; set; }

        [DisplayName("Create Date")]
        public DateTime CreateDate { get; set; }

        [DisplayName("Last Activity")]
        public DateTime LastActivity { get; set; }

        [DisplayName("Ticket Status")]
        public TICKETSTATUS TicketStatusEnumType { get; set; }

        ////////////////////////////

        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }


        public virtual ICollection<TicketDetail> TicketDetails { get; set; }

    }
}