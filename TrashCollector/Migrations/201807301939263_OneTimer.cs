namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OneTimer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OneTimePickups",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        date = c.String(),
                        UserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.OneTimePickups", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.OneTimePickups", new[] { "UserId" });
            DropTable("dbo.OneTimePickups");
        }
    }
}
