namespace MMakerBotPanel.Database.Context
{
    using MMakerBotPanel.Database.Model;
    using System.Data.Entity;

    public class ModelContext : DbContext
    {
        public ModelContext() : base("name=Entities") { }

        public DbSet<PriceUnit> PriceUnits { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductPrice> ProductPrices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Worker> Workers { get; set; }
        public DbSet<WorkerDetail> WorkerDetails { get; set; }
        public DbSet<Faq> Faqs { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketDetail> TicketDetails { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<ExchangeApi> ExchangeApis { get; set; }
        public DbSet<PurchaseHistory> PurchaseHistories { get; set; }
        public DbSet<RiskTest> RiskTests { get; set; }
        public DbSet<ErrorLog> ErrorLogs { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<PaymentLog> PaymentLogs { get; set; }
        public DbSet<ApiLog> ApiLogs { get; set; }
        public DbSet<GridParameter> GridParameters { get; set; }
        public DbSet<GridStrategyLog> GridStrategyLogs { get; set; }
        public DbSet<TickLog> TickLogs { get; set; }
        public DbSet<Dashboard> Dashboards { get; set; }
        public DbSet<ProfitUSDT> ProfitUSDTs { get; set; }
        public DbSet<MakerParameter> MakerParameters { get; set; }
        public DbSet<MakerStrategyLog> MakerStrategyLogs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Worker>().HasMany(e => e.PurchaseHistories).WithRequired(e => e.Worker).WillCascadeOnDelete(false);

            modelBuilder.Entity<User>().HasMany(e => e.Workers).WithRequired(e => e.User).WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasMany(e => e.ExchangeApis).WithRequired(e => e.User).WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasMany(e => e.Tickets).WithRequired(e => e.User).WillCascadeOnDelete(false);
            modelBuilder.Entity<User>().HasMany(e => e.TicketDetails).WithRequired(e => e.User).WillCascadeOnDelete(false);

            modelBuilder.Entity<Ticket>().HasMany(e => e.TicketDetails).WithRequired(e => e.Ticket).WillCascadeOnDelete(false);

            modelBuilder.Entity<Product>().HasMany(e => e.Workers).WithRequired(e => e.Product).WillCascadeOnDelete(false);
            modelBuilder.Entity<Product>().HasMany(e => e.ProductPrices).WithRequired(e => e.Product).WillCascadeOnDelete(false);

            modelBuilder.Entity<PriceUnit>().HasMany(e => e.ProductPrices).WithRequired(e => e.PriceUnit).WillCascadeOnDelete(false);
            modelBuilder.Entity<PriceUnit>().HasMany(e => e.PurchaseHistorys).WithRequired(e => e.PriceUnit).WillCascadeOnDelete(false);


            _ = modelBuilder.Entity<RiskTest>().HasIndex(rt => rt.UserID).IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}