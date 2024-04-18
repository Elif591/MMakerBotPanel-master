namespace MMakerBotPanel.Database.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("TickLog", Schema = "Log")]
    public class TickLog
    {
        [Key]
        public int TickLogID { get; set; }
        public string Timestamp { get; set; }
    }
}