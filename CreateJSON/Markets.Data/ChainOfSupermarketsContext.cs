namespace Markets.Data
{
    using System.Data.Entity;

    using Markets.Data.Migrations;
    using Markets.Model;
   
    public class ChainOfSupermarketsContext : DbContext
    {
        public ChainOfSupermarketsContext()
            : base("name=ChainOfSupermarketsContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ChainOfSupermarketsContext, Configuration>());
        }

        public IDbSet<Vendor> Vendors { get; set; }
        public IDbSet<Measure> Measures { get; set; }
        public IDbSet<Product> Products { get; set; }
        public IDbSet<Location> Locations { get; set; }
        public IDbSet<Expense> Expenses { get; set; }
        public IDbSet<Sale> Sales { get; set; }
    }
}