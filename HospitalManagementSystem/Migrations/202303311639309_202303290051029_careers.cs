namespace HospitalManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class _202303290051029_careers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.News",
                c => new
                    {
                        newsID = c.Int(nullable: false, identity: true),
                        newsTitle = c.Int(nullable: false),
                        newsDescription = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.newsID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.News");
        }
    }
}
