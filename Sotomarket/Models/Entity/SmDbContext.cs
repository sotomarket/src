namespace Sotomarket.Models.Entity
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class SmDbContext : DbContext
    {
        public SmDbContext()
            : base("name=SmDbContext")
        {
        }

        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }

        public virtual DbSet<GoodsCategory> GoodsCategories { get; set; }
        public virtual DbSet<GoodsSubCategory> GoodsSubCategories { get; set; }
        public virtual DbSet<Goods> Goods { get; set; }

        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<Income> Incomes { get; set; }
        public virtual DbSet<IncomeItem> IncomeItems { get; set; }


        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderItem> OrderItems { get; set; }

        public virtual DbSet<Sale> Sales { get; set; }
        public virtual DbSet<SaleItem> SaleItems { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoles>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUsers)
                .HasForeignKey(e => e.UserId);



            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.Sales)
                .WithRequired(e => e.Operator)
                .HasForeignKey(e => e.OperatorId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Operator)
                .HasForeignKey(e => e.OperatorId);

            modelBuilder.Entity<AspNetUsers>()
                .HasMany(e => e.Incomes)
                .WithRequired(e => e.Operator)
                .HasForeignKey(e => e.OperatorId);



            modelBuilder.Entity<GoodsSubCategory>()
                .HasMany(e => e.Goods)
                .WithOptional(e => e.GoodsSubCategory)
                .HasForeignKey(e => e.GoodsSubCategoryId);

            modelBuilder.Entity<GoodsCategory>()
                .HasMany(e => e.GoodsSubCategories)
                .WithRequired(e => e.GoodsCategory)
                .HasForeignKey(e => e.GoodsCategoryId);

            modelBuilder.Entity<GoodsCategory>()
                .HasMany(e => e.Goods)
                .WithRequired(e => e.GoodsCategory)
                .HasForeignKey(e => e.GoodsCategoryId);

            modelBuilder.Entity<Goods>()
                .HasMany(e => e.IncomeItems)
                .WithRequired(e => e.Goods)
                .HasForeignKey(e => e.GoodsId);

            modelBuilder.Entity<Goods>()
                .HasMany(e => e.OrderItems)
                .WithRequired(e => e.Goods)
                .HasForeignKey(e => e.GoodsId);

            modelBuilder.Entity<Goods>()
                .HasMany(e => e.SaleItems)
                .WithRequired(e => e.Goods)
                .HasForeignKey(e => e.GoodsId);

            modelBuilder.Entity<Supplier>()
                .HasMany(e => e.Incomes)
                .WithRequired(e => e.Supplier)
                .HasForeignKey(e => e.SupplierId);

            modelBuilder.Entity<Income>()
               .HasMany(e => e.IncomeItems)
               .WithRequired(e => e.Income)
               .HasForeignKey(e => e.IncomeId);

            modelBuilder.Entity<Order>()
               .HasMany(e => e.OrderItems)
               .WithRequired(e => e.Order)
               .HasForeignKey(e => e.OrderId);

            modelBuilder.Entity<OrderItem>()
               .HasMany(e => e.SaleItems)
               .WithRequired(e => e.OrderItem)
               .HasForeignKey(e => e.OrderItemId);

            modelBuilder.Entity<Order>()
               .HasMany(e => e.Sales)
               .WithRequired(e => e.Order)
               .HasForeignKey(e => e.OrderId);

            modelBuilder.Entity<Sale>()
               .HasMany(e => e.SaleItems)
               .WithRequired(e => e.Sale)
               .HasForeignKey(e => e.SaleId);

            
        }
    }
}
