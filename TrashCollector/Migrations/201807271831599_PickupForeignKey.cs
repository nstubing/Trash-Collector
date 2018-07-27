namespace TrashCollector.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PickupForeignKey : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.Pickups", name: "User_Id", newName: "UserId");
            RenameIndex(table: "dbo.Pickups", name: "IX_User_Id", newName: "IX_UserId");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.Pickups", name: "IX_UserId", newName: "IX_User_Id");
            RenameColumn(table: "dbo.Pickups", name: "UserId", newName: "User_Id");
        }
    }
}
