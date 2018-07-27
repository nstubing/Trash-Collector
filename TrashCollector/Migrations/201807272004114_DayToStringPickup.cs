namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DayToStringPickup : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pickups", "date", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pickups", "date", c => c.DateTime(nullable: false));
        }
    }
}
