namespace POEM.Model.Model
{
    using System;
    using System.Data.Entity;
    using System.Linq;

    public class ApplicationDbContext : DbContext
    {
        // Your context has been configured to use a 'ApplicationDbContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'POEM.Model.Model.ApplicationDbContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ApplicationDbContext' 
        // connection string in the application configuration file.
        public DbSet<CollectionDtl> CollectionDtls { get; set; }
        public DbSet<CategoryDetails> CategoryDetails { get; set; }
        public DbSet<VendorDetails> VendorDetails { get; set; }
        public DbSet<MetalKtDetails> MetalKtDetails { get; set; }
        public DbSet<MetelColorDtls> MetelColorDtls { get; set; }
        public DbSet<FindingDetail> findingDetails { get; set; }
        public DbSet<StoneShapeDetail> StoneShapeDetails { get; set; }
        public DbSet<MetalLossDetails> MetalLossDetails { get; set; }
        public DbSet<DiamondDetail> DiamondDetails { get; set; }
        public DbSet<SettingLaborDetail> SettingLaborDetails { get; set; }
        public DbSet<SKUDetailsDbDto> SKUDetails { get; set; }
        public DbSet<SKUMetalDbDto> SKUMetalDetails { get; set; }
        public DbSet<SKUFindingsDbDto> SKUFindingsDetails { get; set; }
        public DbSet<SKUStoneDbDto> SKUStoneDetails { get; set; }
        public DbSet<SKULaborDbDto> SKULaborDetails { get; set; }
        public DbSet<SkuCalculation> SkuCalculations { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<ProcessCostingDetails> ProcessCostingDetails { get; set; }
        public DbSet<CompanyMasterDbDto> CompanyMaster { get; set; }
        public DbSet<SubCategoryMasterDbDto> SubCategoryMasters { get; set; }
        public DbSet<FindingTypeMasterDbDto> FindingTypeMaster { get; set; }
        public DbSet<MarginDetailsDbDto> MarginDetails { get; set; }



        public ApplicationDbContext()
            : base("name=PoemPricingDbModel")
        {
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // SKU Details
            modelBuilder.Entity<SKUDetailsDbDto>()
                .Property(p => p.semiMinWt)
                .HasPrecision(5, 4); // precision, scale
            modelBuilder.Entity<SKUDetailsDbDto>()
                .Property(p => p.centerMinWt)
                .HasPrecision(5, 4); // precision, scale
            modelBuilder.Entity<SKUDetailsDbDto>()
                .Property(p => p.SemiAdjWt)
                .HasPrecision(5, 4); // precision, scale
            modelBuilder.Entity<SKUDetailsDbDto>()
                .Property(p => p.CenterAdjWt)
                .HasPrecision(5, 4); // precision, scale
            // Stone Details
            modelBuilder.Entity<SKUStoneDbDto>()
                .Property(p => p.PerStoneWt)
                .HasPrecision(10,4); // precision, scale
            modelBuilder.Entity<SKUStoneDbDto>()
                .Property(p => p.TotalStoneWt)
                .HasPrecision(10, 4); // precision, scale
            modelBuilder.Entity<SKUStoneDbDto>()
                .Property(p => p.TotalAdjStoneWt)
                .HasPrecision(10, 4); // precision, scale            
        }
        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}