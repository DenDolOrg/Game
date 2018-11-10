namespace MyGame.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class WhiteId : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "WhitePlayerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "WhitePlayerId");
        }
    }
}
