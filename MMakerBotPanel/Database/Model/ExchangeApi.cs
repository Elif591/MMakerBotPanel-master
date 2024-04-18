namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Integration", Schema = "User")]
    public class ExchangeApi
    {
        public int ExchangeApiID { get; set; }
        public EXCHANGEAPIKEY ExchangeApiKey { get; set; }
        public CEXEXCHANGE Exchange { get; set; }
        public string Value { get; set; }
        public string PassPhrase { get; set; }

        /////////////////////////////

        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }


    }
}