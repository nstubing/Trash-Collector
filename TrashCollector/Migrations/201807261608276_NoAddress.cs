namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NoAddress : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.AspNetUsers", "AddressNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "AddressNumber", c => c.Int(nullable: false));
        }
    }
}
