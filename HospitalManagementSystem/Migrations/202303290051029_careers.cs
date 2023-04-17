namespace HospitalManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class careers : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Careers",
                c => new
                    {
                        CareerId = c.Int(nullable: false, identity: true),
                        JobName = c.String(),
                        JobId = c.Int(nullable: false),
                        JobDescription = c.String(),
                        DepartmentId = c.Int(nullable: false),
                        LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.CareerId)
                .ForeignKey("dbo.Departments", t => t.DepartmentId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.LocationId, cascadeDelete: true)
                .Index(t => t.DepartmentId)
                .Index(t => t.LocationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Careers", "LocationId", "dbo.Locations");
            DropForeignKey("dbo.Careers", "DepartmentId", "dbo.Departments");
            DropIndex("dbo.Careers", new[] { "LocationId" });
            DropIndex("dbo.Careers", new[] { "DepartmentId" });
            DropTable("dbo.Careers");
        }
    }
}
