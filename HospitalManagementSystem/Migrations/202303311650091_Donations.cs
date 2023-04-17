namespace HospitalManagementSystem.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Donations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        donationID = c.Int(nullable: false, identity: true),
                        firstName = c.String(),
                        lastName = c.String(),
                        amount = c.Int(nullable: false),
                        date = c.String(),
                    })
                .PrimaryKey(t => t.donationID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Donations");
        }
    }
}
