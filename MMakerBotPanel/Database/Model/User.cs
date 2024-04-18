namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("User", Schema = "User")]
    public class User
    {

        [Key]
        public int UserID { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Surname")]
        public string SurName { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        [DisplayName("Password Again")]
        public string PassaportAgain { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Last Login")]
        public DateTime? LastLogin { get; set; }

        [DisplayName("Role")]
        public string Role { get; set; }

        [DisplayName("Image")]
        public string Image { get; set; }
        public string ImageType { get; set; }
        public bool SuperAdmin { get; set; }
        public bool Disabled { get; set; }
        public USERRISKTYPE riskType { get; set; }

        ////////////////////////////////////////


        public virtual ICollection<Worker> Workers { get; set; }
        public virtual ICollection<ExchangeApi> ExchangeApis { get; set; }
        public virtual ICollection<Ticket> Tickets { get; set; }
        public virtual ICollection<TicketDetail> TicketDetails { get; set; }


        //////////////////////////////////////
        ///
        //public virtual RiskTest RiskTest { get; set; }

    }
}