namespace MyGame.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Table : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Figures", "GameId", "dbo.Games");
            DropIndex("dbo.Figures", new[] { "GameId" });
            AddColumn("dbo.Figures", "TableId", c => c.Int(nullable: false));
            AddColumn("dbo.Games", "BlackPlayerId", c => c.Int(nullable: false));
            CreateIndex("dbo.Figures", "TableId");
            AddForeignKey("dbo.Figures", "TableId", "dbo.Tables", "Id", cascadeDelete: true);
            DropColumn("dbo.Figures", "GameId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Figures", "GameId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Figures", "TableId", "dbo.Tables");
            DropIndex("dbo.Figures", new[] { "TableId" });
            DropColumn("dbo.Games", "BlackPlayerId");
            DropColumn("dbo.Figures", "TableId");
            CreateIndex("dbo.Figures", "GameId");
            AddForeignKey("dbo.Figures", "GameId", "dbo.Games", "Id", cascadeDelete: true);
        }
    }
}
