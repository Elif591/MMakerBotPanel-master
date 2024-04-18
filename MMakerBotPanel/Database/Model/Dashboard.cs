namespace MMakerBotPanel.Database.Model
{
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Dashboard", Schema = "General")]
    public class Dashboard
    {
        public int DashboardID { get; set; }
        public int TotalOrders { get; set; }
        public int ActiveOrders { get; set; }
        public int UserID { get; set; }
    }
}