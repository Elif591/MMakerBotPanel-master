namespace MMakerBotPanel.Database.Model
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("PriceUnit", Schema = "General")]
    public class PriceUnit
    {
        [Key]
        public int PriceUnitID { get; set; }

        [DisplayName("Name")]
        public string Name { get; set; }

        [DisplayName("Short Name")]
        public string ShortName { get; set; }

        [DisplayName("Contract")]
        public string Contract { get; set; }


        /////////////////////////////

        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
        public virtual ICollection<PurchaseHistory> PurchaseHistorys { get; set; }
    }
}