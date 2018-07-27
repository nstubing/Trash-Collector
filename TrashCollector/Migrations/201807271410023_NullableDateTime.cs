namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NullableDateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "ExcludedStartDate", c => c.DateTime());
            AlterColumn("dbo.AspNetUsers", "ExcludedEndDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "ExcludedEndDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.AspNetUsers", "ExcludedStartDate", c => c.DateTime(nullable: false));
        }
    }
}
