namespace MMakerBotPanel.Database.Model
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.UI.WebControls;

    [Table("Contact", Schema = "Contact")]
    public class Contact
    {
        [Key]
        public int ContactID { get; set; }
        [Required]
        public string ContactNumber { get; set; }

        [DisplayName("Name")]
        [Required]
        public string ContactUserName { get; set; }
        [DisplayName("Mesaage")]
        [Required]
        public string ContactUserMessage { get; set; }
        [DisplayName("Email")]
        [Required]
        public string ContactUserEmail { get; set; }

        [DisplayName("")]
        public string ContactUserReply { get; set; }

        public int StatusEnumType { get; set; }
    }

}