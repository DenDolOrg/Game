namespace MyGame.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class lastTurn : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Games", "LastTurnPlayerId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Games", "LastTurnPlayerId");
        }
    }
}
