namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Addedendstart : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ExcludedStartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.AspNetUsers", "ExcludedEndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ExcludedEndDate");
            DropColumn("dbo.AspNetUsers", "ExcludedStartDate");
        }
    }
}
