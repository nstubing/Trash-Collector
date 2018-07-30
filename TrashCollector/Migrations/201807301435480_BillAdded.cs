namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BillAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "BillTotal", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "BillTotal");
        }
    }
}
