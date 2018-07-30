namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZipdToORder : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pickups", "Zipcode", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pickups", "Zipcode");
        }
    }
}
