namespace HospitalManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class departments_locations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LocationDepartments",
                c => new
                    {
                        Location_LocationId = c.Int(nullable: false),
                        Department_DepartmentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Location_LocationId, t.Department_DepartmentId })
                .ForeignKey("dbo.Locations", t => t.Location_LocationId, cascadeDelete: true)
                .ForeignKey("dbo.Departments", t => t.Department_DepartmentId, cascadeDelete: true)
                .Index(t => t.Location_LocationId)
                .Index(t => t.Department_DepartmentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LocationDepartments", "Department_DepartmentId", "dbo.Departments");
            DropForeignKey("dbo.LocationDepartments", "Location_LocationId", "dbo.Locations");
            DropIndex("dbo.LocationDepartments", new[] { "Department_DepartmentId" });
            DropIndex("dbo.LocationDepartments", new[] { "Location_LocationId" });
            DropTable("dbo.LocationDepartments");
        }
    }
}
