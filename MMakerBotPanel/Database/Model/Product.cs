namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Product", Schema = "Product")]
    public class Product
    {
        [Key]
        public int ProductID { get; set; }

        [DisplayName("Product Name")]
        public string ProductName { get; set; }

        [DisplayName("Trial")]
        public bool Demo { get; set; }

        public WORKERTYPE workerType { get; set; }



        //////////////////////////////////////


        public virtual ICollection<Worker> Workers { get; set; }

        public virtual ICollection<ProductPrice> ProductPrices { get; set; }
    }
}