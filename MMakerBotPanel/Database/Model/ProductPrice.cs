namespace MMakerBotPanel.Database.Model
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ProductPrice", Schema = "Product")]
    public class ProductPrice
    {
        [Key]
        public int ProductPriceID { get; set; }
        public float MonthlyPrice { get; set; }
        public float YearlyPrice { get; set; }


        //////////////////////////


        [ForeignKey("Product")]
        [Index("IX_ProductIDAndPriceUnitID", 1, IsUnique = true)]
        public int ProductID { get; set; }
        public virtual Product Product { get; set; }


        [ForeignKey("PriceUnit")]
        [Index("IX_ProductIDAndPriceUnitID", 2, IsUnique = true)]
        public int PriceUnitID { get; set; }
        public virtual PriceUnit PriceUnit { get; set; }

    }
}