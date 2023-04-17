namespace HospitalManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class betterNews : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.News", "newsTitle", c => c.String());
            AlterColumn("dbo.News", "newsDescription", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.News", "newsDescription", c => c.Int(nullable: false));
            AlterColumn("dbo.News", "newsTitle", c => c.Int(nullable: false));
        }
    }
}
