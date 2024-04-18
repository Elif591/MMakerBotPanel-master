namespace MMakerBotPanel.Database.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("RiskTest", Schema = "User")]
    public class RiskTest
    {
        [Key]
        public int RiskID { get; set; }

        [ForeignKey("User")]
        public int UserID { get; set; }
        public int Answer1 { get; set; }
        public int Answer2 { get; set; }
        public int Answer3 { get; set; }
        public int Answer4 { get; set; }
        public int Answer5 { get; set; }
        public int Answer6 { get; set; }
        public int Answer7 { get; set; }
        public int Answer8 { get; set; }
        public int Answer9 { get; set; }
        public int Answer10 { get; set; }


        /////////////////////////////

        // Navigation Property
        public virtual User User { get; set; }
    }
}