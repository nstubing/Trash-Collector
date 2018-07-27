namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PickupDateAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Pickups", "date", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Pickups", "date");
        }
    }
}
