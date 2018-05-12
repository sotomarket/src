namespace Sotomarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMainModel : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Incomes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SupplierId = c.Int(nullable: false),
                        IncomeDate = c.DateTime(nullable: false),
                        DocumentNumber = c.String(),
                        OperatorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Suppliers", t => t.SupplierId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.OperatorId, cascadeDelete: false)
                .Index(t => t.SupplierId)
                .Index(t => t.OperatorId);
            
            CreateTable(
                "dbo.IncomeItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        IncomeId = c.Int(nullable: false),
                        GoodsId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Goods", t => t.GoodsId, cascadeDelete: false)
                .ForeignKey("dbo.Incomes", t => t.IncomeId, cascadeDelete: false)
                .Index(t => t.IncomeId)
                .Index(t => t.GoodsId);
            
            CreateTable(
                "dbo.Goods",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProductCode = c.String(),
                        GoodsCategoryId = c.Int(nullable: false),
                        GoodsSubCategoryId = c.Int(),
                        Name = c.String(),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Quantity = c.Int(nullable: false),
                        Description = c.String(),
                        Brand = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodsCategories", t => t.GoodsCategoryId, cascadeDelete: false)
                .ForeignKey("dbo.GoodsSubCategories", t => t.GoodsSubCategoryId)
                .Index(t => t.GoodsCategoryId)
                .Index(t => t.GoodsSubCategoryId);
            
            CreateTable(
                "dbo.GoodsCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.GoodsSubCategories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        GoodsCategoryId = c.Int(nullable: false),
                        Name = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.GoodsCategories", t => t.GoodsCategoryId, cascadeDelete: false)
                .Index(t => t.GoodsCategoryId);
            
            CreateTable(
                "dbo.OrderItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        GoodsId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: false)
                .ForeignKey("dbo.Goods", t => t.GoodsId, cascadeDelete: false)
                .Index(t => t.OrderId)
                .Index(t => t.GoodsId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ClientName = c.String(),
                        ClientIdentifier = c.String(),
                        ClientAddress = c.String(),
                        ClientDescription = c.String(),
                        OrderDate = c.DateTime(nullable: false),
                        OperatorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.OperatorId, cascadeDelete: false)
                .Index(t => t.OperatorId);
            
            CreateTable(
                "dbo.Sales",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(nullable: false),
                        RealizationDate = c.DateTime(nullable: false),
                        Paytype = c.String(),
                        OperatorId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId, cascadeDelete: false)
                .ForeignKey("dbo.AspNetUsers", t => t.OperatorId, cascadeDelete: false)
                .Index(t => t.OrderId)
                .Index(t => t.OperatorId);
            
            CreateTable(
                "dbo.SaleItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        SaleId = c.Int(nullable: false),
                        OrderItemId = c.Int(nullable: false),
                        GoodsId = c.Int(nullable: false),
                        Amount = c.Int(nullable: false),
                        Price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Discount = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Sales", t => t.SaleId, cascadeDelete: false)
                .ForeignKey("dbo.OrderItems", t => t.OrderItemId, cascadeDelete: false)
                .ForeignKey("dbo.Goods", t => t.GoodsId, cascadeDelete: false)
                .Index(t => t.SaleId)
                .Index(t => t.OrderItemId)
                .Index(t => t.GoodsId);
            
            CreateTable(
                "dbo.Suppliers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Identifier = c.String(),
                        Address = c.String(),
                        Description = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Sales", "OperatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Orders", "OperatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Incomes", "OperatorId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Incomes", "SupplierId", "dbo.Suppliers");
            DropForeignKey("dbo.IncomeItems", "IncomeId", "dbo.Incomes");
            DropForeignKey("dbo.SaleItems", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.OrderItems", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.SaleItems", "OrderItemId", "dbo.OrderItems");
            DropForeignKey("dbo.Sales", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.SaleItems", "SaleId", "dbo.Sales");
            DropForeignKey("dbo.OrderItems", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.IncomeItems", "GoodsId", "dbo.Goods");
            DropForeignKey("dbo.GoodsSubCategories", "GoodsCategoryId", "dbo.GoodsCategories");
            DropForeignKey("dbo.Goods", "GoodsSubCategoryId", "dbo.GoodsSubCategories");
            DropForeignKey("dbo.Goods", "GoodsCategoryId", "dbo.GoodsCategories");
            DropIndex("dbo.SaleItems", new[] { "GoodsId" });
            DropIndex("dbo.SaleItems", new[] { "OrderItemId" });
            DropIndex("dbo.SaleItems", new[] { "SaleId" });
            DropIndex("dbo.Sales", new[] { "OperatorId" });
            DropIndex("dbo.Sales", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "OperatorId" });
            DropIndex("dbo.OrderItems", new[] { "GoodsId" });
            DropIndex("dbo.OrderItems", new[] { "OrderId" });
            DropIndex("dbo.GoodsSubCategories", new[] { "GoodsCategoryId" });
            DropIndex("dbo.Goods", new[] { "GoodsSubCategoryId" });
            DropIndex("dbo.Goods", new[] { "GoodsCategoryId" });
            DropIndex("dbo.IncomeItems", new[] { "GoodsId" });
            DropIndex("dbo.IncomeItems", new[] { "IncomeId" });
            DropIndex("dbo.Incomes", new[] { "OperatorId" });
            DropIndex("dbo.Incomes", new[] { "SupplierId" });
            DropTable("dbo.Suppliers");
            DropTable("dbo.SaleItems");
            DropTable("dbo.Sales");
            DropTable("dbo.Orders");
            DropTable("dbo.OrderItems");
            DropTable("dbo.GoodsSubCategories");
            DropTable("dbo.GoodsCategories");
            DropTable("dbo.Goods");
            DropTable("dbo.IncomeItems");
            DropTable("dbo.Incomes");
        }
    }
}
