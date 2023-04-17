namespace HospitalManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class serviceslocations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ServiceLocations",
                c => new
                    {
                        Service_ServiceId = c.Int(nullable: false),
                        Location_LocationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Service_ServiceId, t.Location_LocationId })
                .ForeignKey("dbo.Services", t => t.Service_ServiceId, cascadeDelete: true)
                .ForeignKey("dbo.Locations", t => t.Location_LocationId, cascadeDelete: true)
                .Index(t => t.Service_ServiceId)
                .Index(t => t.Location_LocationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ServiceLocations", "Location_LocationId", "dbo.Locations");
            DropForeignKey("dbo.ServiceLocations", "Service_ServiceId", "dbo.Services");
            DropIndex("dbo.ServiceLocations", new[] { "Location_LocationId" });
            DropIndex("dbo.ServiceLocations", new[] { "Service_ServiceId" });
            DropTable("dbo.ServiceLocations");
        }
    }
}
