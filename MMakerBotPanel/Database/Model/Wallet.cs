namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Wallet", Schema = "General")]
    public class Wallet
    {
        [Key]
        public int WalletID { get; set; }

        [DisplayName("Wallet Address")]
        public string WalletAddress { get; set; }
        [DisplayName("Etherscan API Key")]
        public string EtherScanApiKey { get; set; }
        [DisplayName("TTL")]
        public int TtlCount { get; set; }
        public CHECKDATESTATUS CheckPeriod { get; set; }


    }
}