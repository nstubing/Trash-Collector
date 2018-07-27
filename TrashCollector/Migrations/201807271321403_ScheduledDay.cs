namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ScheduledDay : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "ScheduledDay", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "ScheduledDay");
        }
    }
}
