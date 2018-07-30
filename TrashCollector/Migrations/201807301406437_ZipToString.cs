namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ZipToString : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Pickups", "Zipcode", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Pickups", "Zipcode", c => c.Int(nullable: false));
        }
    }
}
