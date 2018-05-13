namespace Sotomarket.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProcessedFields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Incomes", "Processed", c => c.Boolean(nullable: false));
            AddColumn("dbo.Sales", "Processed", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Sales", "Processed");
            DropColumn("dbo.Incomes", "Processed");
        }
    }
}
