namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class StateAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
            AddColumn("dbo.AspNetUsers", "State", c => c.String());
            DropColumn("dbo.AspNetUsers", "StreetAddress");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AspNetUsers", "StreetAddress", c => c.String());
            DropColumn("dbo.AspNetUsers", "State");
            DropColumn("dbo.AspNetUsers", "Address");
        }
    }
}
