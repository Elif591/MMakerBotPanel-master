namespace MMakerBotPanel.Database.Model
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Web.UI.WebControls;

    [Table("Faq", Schema = "Faq")]
    public class Faq
    {
        [Key]
        public int FaqID { get; set; }

        [DisplayName("Question")]
        [MaxLength(200)]
        [Required]
        public string Question { get; set; }

        [DisplayName("Answer")]
        [MaxLength(1000)]
        [Required]
        public string Answer { get; set; }

        [DisplayName("Panel")]
        public bool Panel { get; set; }

        [DisplayName("HelpDesk")]
        public bool HelpDesk { get; set; }

        [DisplayName("Home")]
        public bool Home { get; set; }
    }
}