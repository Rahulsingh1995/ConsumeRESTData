namespace CustomerSite
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CSModel : DbContext
    {
        public CSModel()
            : base("name=CSModel")
        {
        }

        public virtual DbSet<customer> customers { get; set; }
        public virtual DbSet<order> orders { get; set; }
        public virtual DbSet<product> products { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<customer>()
                .Property(e => e.cust_name)
                .IsUnicode(false);

            modelBuilder.Entity<customer>()
                .Property(e => e.contact_no)
                .HasPrecision(10, 0);

            modelBuilder.Entity<customer>()
                .HasMany(e => e.orders)
                .WithRequired(e => e.customer)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<product>()
                .Property(e => e.product_name)
                .IsUnicode(false);

            modelBuilder.Entity<product>()
                .Property(e => e.product_price)
                .HasPrecision(5, 2);

            modelBuilder.Entity<product>()
                .HasMany(e => e.orders)
                .WithRequired(e => e.product)
                .WillCascadeOnDelete(false);
        }
    }
}
