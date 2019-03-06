namespace MyGame.DAL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class testMigr : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Figures", "IsSuperFigure", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Figures", "IsSuperFigure");
        }
    }
}
