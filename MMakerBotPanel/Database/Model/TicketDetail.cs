namespace MMakerBotPanel.Database.Model
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TicketDetail", Schema = "Ticket")]
    public class TicketDetail
    {
        [Key]
        public int TicketDetayID { get; set; }

        [DisplayName("Message")]
        public string Message { get; set; }

        [DisplayName("Date")]
        public DateTime Date { get; set; }



        //////////////////////////////


        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }


        [ForeignKey("Ticket")]
        public int TicketID { get; set; }
        public virtual Ticket Ticket { get; set; }
    }
}